using System;
using System.Linq;
using System.Collections.Concurrent;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.Dependency
{
    public class SimpleContainer : IContainer
    {
        private readonly object syncLock = new object();
        private readonly ConcurrentDictionary<Type, object> factories;       
        private readonly ConcurrentDictionary<Type, Type> registrations;
        private readonly ConcurrentDictionary<Type, object> instances;

        public SimpleContainer()
        {
            factories = new ConcurrentDictionary<Type, object>();
            registrations = new ConcurrentDictionary<Type, Type>();
            instances = new ConcurrentDictionary<Type, object>();
        }        

        public virtual IServiceRegister Register<TService>(
            Func<IServiceProvider, TService> serviceFactory,
            DependencyLifecycle lifecycle = DependencyLifecycle.Singleton)
            where TService : class
        {
            GuardHelper.ArgumentNotNull(() => serviceFactory);
            CheckDependencyLifecycle(lifecycle);
            var serviceType = typeof(TService);
            if (ServiceIsRegistered(serviceType))
            {
                return this;
            }
            lock (syncLock)
            {
                if (lifecycle == DependencyLifecycle.Singleton)
                {
                    var service = serviceFactory(this);
                    instances.TryAdd(serviceType, service);
                }
                else
                {
                    factories.TryAdd(serviceType, serviceFactory);
                }
            }
            return this;
        }

        public IServiceRegister Register<TService, TImplementation>(
            DependencyLifecycle lifecycle = DependencyLifecycle.Singleton)
            where TService : class
            where TImplementation : class, TService
        {
            CheckDependencyLifecycle(lifecycle);
            var serviceType = typeof(TService);
            var implementationType = typeof(TImplementation);
            if (ServiceIsRegistered(serviceType))
            {
                return this;
            }
            CheckServiceType(serviceType, implementationType);
            lock (syncLock)
            {
                if (lifecycle == DependencyLifecycle.Singleton)
                {
                    var service = CreateServiceInstance(implementationType);
                    instances.TryAdd(serviceType, service);
                }
                else
                {
                    registrations.TryAdd(serviceType, implementationType);
                }
                
            }
            return this;
        }       

        public virtual TService Resolve<TService>() where TService : class
        {
            var serviceType = typeof(TService);
            object service;

            if (!instances.TryGetValue(serviceType, out service))
            {
                if (registrations.ContainsKey(serviceType))
                {
                    var implementationType = registrations[serviceType];
                    service = CreateServiceInstance(implementationType);
                }
                else if (factories.ContainsKey(serviceType))
                {
                    service = ((Func<IServiceProvider, TService>)factories[serviceType])(this);
                }
                else
                {
                    throw new FrameworkException("No service of type {0} has been registered", serviceType.Name);
                }
            }
            return (TService)service;
        }

        private void CheckServiceType(Type serviceType, Type implementationType)
        {
            if (!serviceType.IsAssignableFrom(implementationType))
            {
                var msg = "Component {0} does not implement service interface {1}";
                throw new FrameworkException(msg, implementationType.Name, serviceType.Name);
            }

            var constructors = implementationType.GetConstructors();
            if (constructors.Length != 1)
            {
                var msg = "A service must have one and only one constructor. Service {0} has {1}";
                throw new FrameworkException(msg, implementationType.Name, constructors.Length.ToString());
            }
        }

        private bool ServiceIsRegistered(Type serviceType)
        {
            return instances.ContainsKey(serviceType)
                || factories.ContainsKey(serviceType)
                || registrations.ContainsKey(serviceType);
        }

        private void CheckDependencyLifecycle(DependencyLifecycle lifecycle)
        {
            if (lifecycle != DependencyLifecycle.Singleton && lifecycle != DependencyLifecycle.Transient)
            {
                var msg = "Simple container only support to register singleton and transient lifecycle!";
                throw new FrameworkException(msg);
            }
        }

        private object CreateServiceInstance(Type implementationType)
        {
            var constructors = implementationType.GetConstructors();

            var parameters = constructors[0]
                .GetParameters()
                .Select(parameterInfo => Resolve(parameterInfo.ParameterType))
                .ToArray();

            return constructors[0].Invoke(parameters);
        }

        private object Resolve(Type serviceType)
        {
            return typeof(SimpleContainer)
                .GetMethod("Resolve", new Type[0])
                .MakeGenericMethod(serviceType)
                .Invoke(this, new object[0]);
        }
    }
}
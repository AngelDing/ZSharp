using Microsoft.Practices.Unity;
using System;
using ZSharp.Framework.Dependency;

namespace ZSharp.Framework.Infrastructure
{
    public class CustomUnityContainer : IContainer
    {
        private static IUnityContainer container = new UnityContainer();

        public static IUnityContainer GetConfiguredContainer()
        {
            return container;
        }

        public IServiceRegister Register<TService>(
            Func<Dependency.IServiceProvider, TService> serviceFactory, 
            DependencyLifecycle lifecycle = DependencyLifecycle.Singleton)
            where TService : class
        {
            var svc = serviceFactory(this);
            var lifetimeManager = GetLifetimeManager(lifecycle);
            container.RegisterInstance(svc, lifetimeManager);
            return this;
        }

        private LifetimeManager GetLifetimeManager(DependencyLifecycle lifecycle)
        {
            LifetimeManager manager = new ContainerControlledLifetimeManager();
            switch (lifecycle)
            {
                case DependencyLifecycle.Transient:
                    manager = new TransientLifetimeManager();
                    break;
                default:
                    break;
            }
            return manager;
        }

        public TService Resolve<TService>() where TService : class
        {
            return container.Resolve<TService>();
        }

        public IServiceRegister Register<TService, TImplementation>(
            DependencyLifecycle lifecycle = DependencyLifecycle.Singleton)
            where TService : class
            where TImplementation : class, TService
        {
            var lifetimeManager = GetLifetimeManager(lifecycle);
            container.RegisterType<TService, TImplementation>(lifetimeManager);
            return this;
        }
    }
}

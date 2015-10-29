using System;
using System.Collections.Concurrent;

namespace ZSharp.Framework.Dependency
{
    public class SimpleContainer : IContainer
    {
        private readonly ConcurrentDictionary<Type, object> factories;

        public SimpleContainer()
        {
            factories = new ConcurrentDictionary<Type, object>();
        }

        public virtual void Register<TService>(Func<TService> factory)
        {
            Type key = typeof(TService);
            factories[key] = factory;
        }

        public virtual TService Resolve<TService>()
        {
            object factory;

            if (factories.TryGetValue(typeof(TService), out factory))
            {
                return ((Func<TService>)factory)();
            }

            Type serviceType = typeof(TService);
            if (serviceType.IsInterface || serviceType.IsAbstract)
            {
                return default(TService);
            }

            try
            {
                return (TService)Activator.CreateInstance(serviceType);
            }
            catch
            {
                return default(TService);
            }
        }
    }
}
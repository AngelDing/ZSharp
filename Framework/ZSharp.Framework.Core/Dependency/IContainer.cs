using System;

namespace ZSharp.Framework.Dependency
{
    /// <summary>
    /// Represents an implementation of an IoC container. 
    /// Implement this interface To wrap your favorite IoC container.
    /// </summary>
    public interface IContainer : IServiceProvider, IServiceRegister
    {
    }

    /// <summary>
    /// Provides service instances
    /// </summary>
    public interface IServiceProvider
    {
        /// <summary>
        /// Get an instance of the requested services. Note all services are singletons; multiple calls
        /// to Resolve will all return the same instance.
        /// </summary>
        /// <typeparam name="TService">The type of serivce to return</typeparam>
        /// <returns>The single instance of the service</returns>
        TService Resolve<TService>() where TService : class;
    }

    /// <summary>
    /// Register services
    /// </summary>
    public interface IServiceRegister
    {
        /// <summary>
        /// Register a service with a factory method. Note that the first registration wins. 
        /// All subsequent registrations will be ignored.
        /// </summary>
        /// <typeparam name="TService">The type of the service to be registered</typeparam>
        /// <param name="serviceFactory">A function that can create an instance of the service</param>
        /// <param name="lifecycle">A dependency lifecycle, default is singleton</param>
        /// <returns>itself for nice fluent composition</returns>
        IServiceRegister Register<TService>(
            Func<IServiceProvider, TService> serviceFactory,
            DependencyLifecycle lifecycle = DependencyLifecycle.Singleton)
            where TService : class;

        /// <summary>
        /// Register a service. Note that the first registration wins.
        /// All subsequent registrations will be ignored.
        /// </summary>
        /// <typeparam name="TService">The type of the service to be registered</typeparam>
        /// <typeparam name="TImplementation">The implementation type</typeparam>
        /// <param name="lifecycle">A dependency lifecycle, default is singleton</param>
        /// <returns>itself for nice fluent composition</returns>
        IServiceRegister Register<TService, TImplementation>(
            DependencyLifecycle lifecycle = DependencyLifecycle.Singleton)
            where TService : class
            where TImplementation : class, TService;
    }
}
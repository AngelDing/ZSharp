using System;
using ZSharp.Framework.Dependency;
using ZSharp.Framework.Infrastructure;

namespace ZSharp.Framework.RabbitMq
{
    public static class RabbitHutch
    {
        private static Func<IContainer> createContainerInternal = () => new CustomUnityContainer();

        public static IBus CreateBus(Action<IServiceRegister> registerServices)
        {
            var container = createContainerInternal();
            ServiceLocator.SetLocatorProvider(CustomUnityContainer.GetConfiguredContainer());

            var connectionConfiguration = GetConnectionConfiguration();
            container.Register(p => connectionConfiguration);
            container.Register(p => AdvancedBusEventHandlers.Default);
            registerServices(container);
            ComponentRegistration.RegisterServices(container);
            return container.Resolve<IBus>();
        }

        private static ConnectionConfiguration GetConnectionConfiguration()
        {
            throw new NotImplementedException();
        }
    }
}

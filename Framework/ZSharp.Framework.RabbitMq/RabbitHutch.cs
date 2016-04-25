using System;
using ZSharp.Framework.Configurations;
using ZSharp.Framework.Dependency;
using ZSharp.Framework.Infrastructure;
using ZSharp.Framework.Extensions;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.RabbitMq
{
    public static class RabbitHutch
    {
        private static Func<IContainer> createContainerInternal = () => new CustomUnityContainer();

        public static IBus CreateBus(string rabbitMqConfigName = null)
        {
            return CreateBus(rabbitMqConfigName, c => { });
        }

        public static IBus CreateBus(string rabbitMqConfigName, Action<IServiceRegister> registerServices)
        {
            var container = createContainerInternal();
            ServiceLocator.SetLocatorProvider(CustomUnityContainer.GetConfiguredContainer());

            var connectionConfiguration = GetConnectionConfiguration(rabbitMqConfigName);
            container.Register<IRabbitMqConfiguration>(p => connectionConfiguration);
            container.Register(p => AdvancedBusEventHandlers.Default);
            registerServices(container);
            ComponentRegistration.RegisterServices(container);
            return container.Resolve<IBus>();
        }

        private static RabbitMqConfiguration GetConnectionConfiguration(string rabbitMqConfigName)
        {
            if (rabbitMqConfigName.IsNullOrEmpty())
            {
                rabbitMqConfigName = CommonConfig.RabbitMqConfigName;
            }
            return new RabbitMqConfiguration(rabbitMqConfigName);
        }
    }
}

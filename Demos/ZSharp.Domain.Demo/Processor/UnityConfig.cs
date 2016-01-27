using System;
using Microsoft.Practices.Unity;
using ZSharp.Framework.Domain;
using ZSharp.Framework.Infrastructure;
using System.Collections.Generic;

namespace ZSharp.Domain.Demo
{
    public class UnityConfig
    {
        #region Unity Container

        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }

        #endregion

        public static void RegisterTypes(IUnityContainer container)
        {
            ServiceLocator.SetLocatorProvider(container);

            container.RegisterType<IReadModelFacade, ReadModelFacade>();

            container.RegisterType<IMessageSender, MemoryMessageSender>(
                "CommandMessageSender",
                new TransientLifetimeManager());

            container.RegisterType<IMessageReceiver, MemoryMessageReceiver>(
                "CommandMessageReceiver",
                new TransientLifetimeManager(),
                new InjectionConstructor(new List<string> { Constants.ApplicationRuntime.DefaultCommandTopic }));

            container.RegisterType<IMessageSender, MemoryMessageSender>(
                "EventMessageSender",
                new TransientLifetimeManager());

            container.RegisterType<IMessageReceiver, MemoryMessageReceiver>(
                "EventMessageReceiver",
                new TransientLifetimeManager(),
                new InjectionConstructor(new List<string> { Constants.ApplicationRuntime.DefaultEventTopic }));

            var commandBus = new CommandBus(container.Resolve<IMessageSender>("CommandMessageSender"));
            var eventBus = new EventBus(container.Resolve<IMessageSender>("EventMessageSender"));

            var commandProcessor = new MessageProcessor(container.Resolve<IMessageReceiver>("CommandMessageReceiver"));
            var eventProcessor = new MessageProcessor(container.Resolve<IMessageReceiver>("EventMessageReceiver"));

            container.RegisterInstance<ICommandBus>(commandBus);
            container.RegisterInstance<IEventBus>(eventBus);


            container.RegisterType(typeof(IDomainEventRepository<>), typeof(MemoryDomainEventRepository<>));
            //container.RegisterType(typeof(ISnapshotRepository<>), typeof(NullSnapshotRepository<>));
            container.RegisterType(typeof(ISnapshotRepository<>), typeof(MemorySnapshotRepository<>));
            //container.RegisterType(typeof(ISnapshotPolicy), typeof(NoSnapshotPolicy));
            container.RegisterType(
                typeof(ISnapshotPolicy),
                typeof(SimpleSnapshotPolicy),
                new InjectionConstructor(Constants.ApplicationRuntime.DefaultSnapshotIntervalInEvents));

            container.RegisterType(typeof(IEventStore<>), typeof(EventStore<>));

            container.RegisterInstance<IProcessor>("CommandProcessor", commandProcessor);
            container.RegisterInstance<IProcessor>("EventProcessor", eventProcessor);

            // Event log database and handler.
            //container.RegisterType<IMessageLogRepository, NullMessageLogRepository>();
            container.RegisterType<IMessageLogRepository, MemoryMessageLogRepository>();

            RegisterCommandHandlers(container, commandProcessor);
            RegisterEventHandlers(container, eventProcessor);
        }

        private static void RegisterCommandHandlers(IUnityContainer container, IHandlerRegistry registry)
        {
            registry.Register(container.Resolve<InventoryCommandHandler>());
            registry.Register(container.Resolve<MessageLogHandler>());
        }

        private static void RegisterEventHandlers(IUnityContainer container, IHandlerRegistry registry)
        {
            registry.Register(container.Resolve<InventoryListViewCreator>());
            registry.Register(container.Resolve<InvenotryItemDetailViewCreator>());
            registry.Register(container.Resolve<MessageLogHandler>());
        }
    }
}

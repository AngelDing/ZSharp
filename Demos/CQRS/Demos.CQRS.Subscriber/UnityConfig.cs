using System;
using Microsoft.Practices.Unity;
using System.Data.Entity;
using Demos.CQRS.Common;
using ZSharp.Framework.Repositories;
using ZSharp.Framework.SqlDb;
using ZSharp.Framework.Domain;
using ZSharp.Framework.Serializations;

namespace Demos.CQRS.Subscriber
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
            container.RegisterType<DbContext, CqrsDemoContext>()
                .RegisterType<IRepositoryContext, EfRepositoryContext>()
                .RegisterType<ICustomerRepository, CustomerRepository>();

            container.RegisterInstance<ISerializer>(SerializationHelper.Jil);

            container.RegisterType(
                typeof(IMessageRepository<>),
                typeof(SqlMessageRepository<>)
            );

            container.RegisterType<IMessageSender, SqlMessageSender<CommandMessageEntity>>(
                "CommandSender",
                new TransientLifetimeManager(),
                new InjectionConstructor(new ResolvedParameter<IMessageRepository<CommandMessageEntity>>(), typeof(ISerializer)));
            container.RegisterType<IMessageSender, SqlMessageSender<EventMessageEntity>>(
                "EventSender",
                new TransientLifetimeManager(),
                new InjectionConstructor(new ResolvedParameter<IMessageRepository<EventMessageEntity>>(), typeof(ISerializer)));

            container.RegisterType<IMessageReceiver, SqlMessageReceiver<CommandMessageEntity>>(
               "CommandReceiver",
               new TransientLifetimeManager(),
               new InjectionConstructor(new ResolvedParameter<IMessageRepository<CommandMessageEntity>>(), "Common", "Customer"));
            container.RegisterType<IMessageReceiver, SqlMessageReceiver<EventMessageEntity>>(
                "EventReceiver",
                new TransientLifetimeManager(),
                new InjectionConstructor(new ResolvedParameter<IMessageRepository<EventMessageEntity>>(), "Common", "Customer"));

            var commandBus = new CommandBus(container.Resolve<IMessageSender>("CommandSender"));
            var eventBus = new EventBus(container.Resolve<IMessageSender>("EventSender"));

            var commandProcessor = new MessageProcessor(container.Resolve<IMessageReceiver>("CommandReceiver"));
            var eventProcessor = new MessageProcessor(container.Resolve<IMessageReceiver>("EventReceiver"));

            container.RegisterInstance<ICommandBus>(commandBus);
            container.RegisterInstance<IEventBus>(eventBus);


            container.RegisterInstance<IProcessor>("CommandProcessor", commandProcessor);
            container.RegisterInstance<IProcessor>("EventProcessor", eventProcessor);

            // Event log database and handler.
            container.RegisterType<IMessageLogRepository, SqlMessageLogRepository>();

            RegisterHandlers(container, eventProcessor);
            RegisterHandlers(container, commandProcessor);
        }

        private static void RegisterHandlers(IUnityContainer container, IHandlerRegistry registry)
        {
            registry.Register(container.Resolve<CustomerViewGenerator>());
            registry.Register(container.Resolve<MessageLogHandler>());
            registry.Register(container.Resolve<CustomerHandler>());
        }
    }
}

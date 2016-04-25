using ZSharp.Framework.Dependency;
using ZSharp.Framework.Serializations;

namespace ZSharp.Framework.RabbitMq
{
    public class ComponentRegistration
    {
        public static void RegisterServices(IContainer container)
        { 
            container
                .Register(p => container)
                .Register<IConventions, Conventions>()
                .Register<IEventBus, EventBus>()
                .Register<ITypeNameSerializer, TypeNameSerializer>()
                .Register<ICorrelationIdGenerationStrategy, DefaultCorrelationIdGenerationStrategy>()
                .Register<IMessageSerializationStrategy, DefaultMessageSerializationStrategy>()
                .Register<IMessageDeliveryModeStrategy, MessageDeliveryModeStrategy>()
                //.Register<ITimeoutStrategy, TimeoutStrategy>()
                .Register<IClusterHostSelectionStrategy<ConnectionFactoryInfo>, RandomClusterHostSelectionStrategy<ConnectionFactoryInfo>>()
                .Register<IProduceConsumeInterceptor, DefaultInterceptor>()
                //.Register<IConsumerDispatcherFactory, ConsumerDispatcherFactory>()
                .Register<IPublishExchangeDeclareStrategy, PublishExchangeDeclareStrategy>()
                //.Register<IConsumerErrorStrategy, DefaultConsumerErrorStrategy>()
                //.Register<IHandlerRunner, HandlerRunner>()
                //.Register<IInternalConsumerFactory, InternalConsumerFactory>()
                //.Register<IConsumerFactory, ConsumerFactory>()
                .Register<IConnectionFactory, ConnectionFactoryWrapper>()
                .Register<IPersistentChannelFactory, PersistentChannelFactory>()
                .Register<IClientCommandDispatcherFactory, ClientCommandDispatcherFactory>()
                .Register<IPublishConfirmationListener, PublishConfirmationListener>()
                //.Register<IHandlerCollectionFactory, HandlerCollectionFactory>()
                .Register<IAdvancedBus, RabbitAdvancedBus>()
                .Register<ISendReceive, SendReceive>()
                //.Register<IScheduler, ExternalScheduler>()
                .Register<IBus, RabbitBus>();
        }

    }
}
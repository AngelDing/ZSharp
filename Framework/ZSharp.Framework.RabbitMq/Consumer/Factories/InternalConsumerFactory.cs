using System;

namespace ZSharp.Framework.RabbitMq
{
    public interface IInternalConsumerFactory : IDisposable
    {
        IInternalConsumer CreateConsumer();

        void OnDisconnected();
    }

    public class InternalConsumerFactory : BaseRabbitMq, IInternalConsumerFactory
    {
        private readonly IHandlerRunner handlerRunner;
        private readonly IConsumerDispatcherFactory consumerDispatcherFactory;

        public InternalConsumerFactory(
            IHandlerRunner handlerRunner, 
            IConsumerDispatcherFactory consumerDispatcherFactory)
        {
            this.handlerRunner = handlerRunner;
            this.consumerDispatcherFactory = consumerDispatcherFactory;
        }

        public IInternalConsumer CreateConsumer()
        {
            var dispatcher = consumerDispatcherFactory.GetConsumerDispatcher();
            return new InternalConsumer(handlerRunner, dispatcher);
        }

        public void OnDisconnected()
        {
            consumerDispatcherFactory.OnDisconnected();
        }

        public void Dispose()
        {
            consumerDispatcherFactory.Dispose();
            handlerRunner.Dispose();
        }
    }
}
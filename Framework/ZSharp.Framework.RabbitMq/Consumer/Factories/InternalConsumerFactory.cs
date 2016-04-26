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
        private readonly IConventions conventions;
        private readonly IConsumerDispatcherFactory consumerDispatcherFactory;

        public InternalConsumerFactory(
            IHandlerRunner handlerRunner, 
            IConventions conventions, 
            IConsumerDispatcherFactory consumerDispatcherFactory)
        {
            this.handlerRunner = handlerRunner;
            this.conventions = conventions;
            this.consumerDispatcherFactory = consumerDispatcherFactory;
        }

        public IInternalConsumer CreateConsumer()
        {
            var dispatcher = consumerDispatcherFactory.GetConsumerDispatcher();
            return new InternalConsumer(handlerRunner, dispatcher, conventions);
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
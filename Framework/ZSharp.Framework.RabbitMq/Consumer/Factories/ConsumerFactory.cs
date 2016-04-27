using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ZSharp.Framework.RabbitMq
{
    public interface IConsumerFactory : IDisposable
    {
        IConsumer CreateConsumer(
            IQueue queue,
            Func<byte[], MessageProperties, MessageReceivedInfo, Task> onMessage,
            IPersistentConnection connection,
            IConsumerConfiguration configuration);
    }

    public class ConsumerFactory : IConsumerFactory
    {
        private readonly IEventBus eventBus;
        private readonly IInternalConsumerFactory internalConsumerFactory;
        private readonly ConcurrentDictionary<IConsumer, object> consumers;

        public ConsumerFactory(IInternalConsumerFactory internalConsumerFactory, IEventBus eventBus)
        {
            this.internalConsumerFactory = internalConsumerFactory;
            this.eventBus = eventBus;
            consumers = new ConcurrentDictionary<IConsumer, object>();
            eventBus.Subscribe<StoppedConsumingEvent>(stoppedConsumingEvent =>
                {
                    object value;
                    consumers.TryRemove(stoppedConsumingEvent.Consumer, out value);
                });
        }

        public IConsumer CreateConsumer(
            IQueue queue, 
            Func<byte[], MessageProperties, MessageReceivedInfo, Task> onMessage, 
            IPersistentConnection connection, 
            IConsumerConfiguration configuration)
        {
            var consumer = CreateConsumerInstance(queue, onMessage, connection, configuration);
            consumers.TryAdd(consumer, null);
            return consumer;
        }

        /// <summary>
        /// Create the correct implementation of IConsumer based on queue properties
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="onMessage"></param>
        /// <param name="connection"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private IConsumer CreateConsumerInstance(
            IQueue queue, 
            Func<byte[], MessageProperties, MessageReceivedInfo, Task> onMessage, 
            IPersistentConnection connection,
            IConsumerConfiguration configuration)
        {
            if (queue.IsExclusive)
            {
                return new TransientConsumer(queue, onMessage, connection, configuration, internalConsumerFactory, eventBus);
            }
            if (configuration.IsExclusive)
            {
                return new ExclusiveConsumer(queue, onMessage, connection, configuration, internalConsumerFactory, eventBus);
            }
            return new PersistentConsumer(queue, onMessage, connection, configuration, internalConsumerFactory, eventBus);
        }

        public void Dispose()
        {
            foreach (var consumer in consumers.Keys)
            {
                consumer.Dispose();
            }
            internalConsumerFactory.Dispose();
        }
    }
}
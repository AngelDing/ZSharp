using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ZSharp.Framework.RabbitMq
{
    public interface IInternalConsumer : IDisposable
    {
        StartConsumingStatus StartConsuming(
            IPersistentConnection connection,
            IQueue queue,
            Func<byte[], MessageProperties, MessageReceivedInfo, Task> onMessage,
            IConsumerConfiguration configuration);

        event Action<IInternalConsumer> Cancelled;
    }

    public class InternalConsumer : BaseRabbitMq, IBasicConsumer, IInternalConsumer
    {
        private readonly IHandlerRunner handlerRunner;
        private readonly IConsumerDispatcher consumerDispatcher;
        private Func<byte[], MessageProperties, MessageReceivedInfo, Task> onMessage;
        private IQueue queue;

        public IModel Model { get; private set; }
        public event EventHandler<ConsumerEventArgs> ConsumerCancelled;
        public string ConsumerTag { get; private set; }

        public event Action<IInternalConsumer> Cancelled;

        public InternalConsumer(
            IHandlerRunner handlerRunner,
            IConsumerDispatcher consumerDispatcher)
        {
            this.handlerRunner = handlerRunner;
            this.consumerDispatcher = consumerDispatcher;
        }

        public StartConsumingStatus StartConsuming(
            IPersistentConnection connection,
            IQueue queue,
            Func<byte[], MessageProperties, MessageReceivedInfo, Task> onMessage,
            IConsumerConfiguration configuration)
        {
            this.queue = queue;
            this.onMessage = onMessage;
            var consumerTag = Conventions.ConsumerTagConvention();
            IDictionary<string, object> arguments = new Dictionary<string, object>
                {
                    {"x-priority", configuration.Priority},
                    {"x-cancel-on-ha-failover", configuration.CancelOnHaFailover || RabbitMqConfiguration.CancelOnHaFailover}
                };
            try
            {
                Model = connection.CreateModel();

                Model.BasicQos(0, configuration.PrefetchCount, false);

                Model.BasicConsume(
                    queue.Name,         // queue
                    false,              // noAck
                    consumerTag,        // consumerTag
                    true,
                    configuration.IsExclusive,
                    arguments,          // arguments
                    this);              // consumer

                Logger.Info("Declared Consumer. queue='{0}', consumer tag='{1}' prefetchcount={2} priority={3} x-cancel-on-ha-failover={4}",
                                  queue.Name, consumerTag, configuration.PrefetchCount, configuration.Priority, configuration.CancelOnHaFailover);
            }
            catch (Exception exception)
            {
                Logger.Error("Consume failed. queue='{0}', consumer tag='{1}', message='{2}'",
                                 queue.Name, consumerTag, exception.Message);
                return StartConsumingStatus.Failed;
            }
            return StartConsumingStatus.Succeed;
        }

        /// <summary>
        /// Cancel means that an external signal has requested that this consumer should
        /// be cancelled. This is _not_ the same as when an internal consumer stops consuming
        /// because it has lost its channel/connection.
        /// </summary>
        private void Cancel()
        {
            // copy to temp variable to be thread safe.
            var cancelled = Cancelled;
            if(cancelled != null) cancelled(this);

            var consumerCancelled = ConsumerCancelled;
            if(consumerCancelled != null) consumerCancelled(this, new ConsumerEventArgs(ConsumerTag));
        }

        public void HandleBasicConsumeOk(string consumerTag)
        {
            ConsumerTag = consumerTag;
        }

        public void HandleBasicCancelOk(string consumerTag)
        {
            Cancel();
        }

        public void HandleBasicCancel(string consumerTag)
        {
            Cancel();
            Logger.Info("BasicCancel(Consumer Cancel Notification from broker) event received. " +
                             "Consumer tag: " + consumerTag);
        }

        public void HandleModelShutdown(object model, ShutdownEventArgs reason)
        {
            Logger.Info("Consumer '{0}', consuming from queue '{1}', has shutdown. Reason: '{2}'",
                             ConsumerTag, queue.Name, reason.Cause);
        }

        public void HandleBasicDeliver(
            string consumerTag,
            ulong deliveryTag,
            bool redelivered,
            string exchange,
            string routingKey,
            IBasicProperties properties,
            byte[] body)
        {
            Logger.Debug("HandleBasicDeliver on consumer: {0}, deliveryTag: {1}", consumerTag, deliveryTag);

            if (disposed)
            {
                // this message's consumer has stopped, so just return
                Logger.Info("Consumer has stopped running. Consumer '{0}' on queue '{1}'. Ignoring message",
                    ConsumerTag, queue.Name);
                return;
            }

            if (onMessage == null)
            {
                Logger.Error("User consumer callback, 'onMessage' has not been set for consumer '{0}'." +
                    "Please call InternalConsumer.StartConsuming before passing the consumer to basic.consume",
                    ConsumerTag);
                return;
            }

            var messageReceivedInfo = new MessageReceivedInfo(consumerTag, deliveryTag, redelivered, exchange, routingKey, queue.Name);
            var messsageProperties = new MessageProperties(properties);
            var context = new ConsumerExecutionContext(onMessage, messageReceivedInfo, messsageProperties, body, this);

            consumerDispatcher.QueueAction(() =>
                {
                    EventBus.Publish(new DeliveredMessageEvent(messageReceivedInfo, messsageProperties, body));
                    handlerRunner.InvokeUserMessageHandler(context);
                });
        }

        private bool disposed;
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            var model = Model;
            if (model != null)
            {
                // Queued because we may be on the RabbitMQ.Client dispatch thread.
                consumerDispatcher.QueueAction(() =>
                    {
                        Model.Dispose();
                        EventBus.Publish(new ConsumerModelDisposedEvent(ConsumerTag));
                    });
            }
        }
    }
}
using System;
using System.Threading.Tasks;

namespace ZSharp.Framework.RabbitMq
{
    public interface IBus : IDisposable
    {
        void Publish<T>(T message) where T : class;

        void Publish<T>(T message, string topic) where T : class;

        Task PublishAsync<T>(T message) where T : class;

        Task PublishAsync<T>(T message, string topic) where T : class;

        /// <summary>
        /// Subscribes to a stream of messages that match a .NET type.
        /// </summary>
        /// <typeparam name="T">The type to subscribe to</typeparam>
        /// <param name="subscriptionId">
        /// A unique identifier for the subscription. Two subscriptions with the same subscriptionId
        /// and type will get messages delivered in turn. This is useful if you want multiple subscribers
        /// to load balance a subscription in a round-robin fashion.
        /// </param>
        /// <param name="onMessage">
        /// The action to run when a message arrives. When onMessage completes the message
        /// recipt is Ack'd. All onMessage delegates are processed on a single thread so you should
        /// avoid long running blocking IO operations. Consider using SubscribeAsync
        /// </param>
        /// <param name="configure">
        /// Fluent configuration e.g. x => x.WithTopic("uk.london")
        /// </param>
        /// <returns>
        /// An <see cref="ISubscriptionResult"/>
        /// Call Dispose on it or on its <see cref="ISubscriptionResult.ConsumerCancellation"/> to cancel the subscription.
        /// </returns>
        ISubscriptionResult Subscribe<T>(string subscriptionId, Action<T> onMessage, Action<ISubscriptionConfiguration> configure = null)
            where T : class;

        ISubscriptionResult SubscribeAsync<T>(string subscriptionId, Func<T, Task> onMessage, Action<ISubscriptionConfiguration> configure = null)
            where T : class;

        void Send<T>(string queue, T message) where T : class;

        Task SendAsync<T>(string queue, T message) where T : class;

        /// <summary>
        /// Receive messages from a queue,Multiple calls to Receive for the same queue, 
        /// but with different message types will add multiple message handlers to the same consumer.
        /// </summary>
        /// <typeparam name="T">The type of message to receive</typeparam>
        /// <param name="queue">The queue to receive from</param>
        /// <param name="onMessage">The message handler</param>
        /// <param name="configure">Action to configure consumer with</param>
        IDisposable Receive<T>(string queue, Action<T> onMessage, Action<IConsumerConfiguration> configure = null) where T : class;

        /// <summary>
        /// Receive messages from a queue,Multiple calls to Receive for the same queue, 
        /// but with different message types will add multiple message handlers to the same consumer.
        /// </summary>
        /// <typeparam name="T">The type of message to receive</typeparam>
        /// <param name="queue">The queue to receive from</param>
        /// <param name="onMessage">The asychronous message handler</param>
        /// <param name="configure">Action to configure consumer with</param>
        IDisposable ReceiveAsync<T>(string queue, Func<T, Task> onMessage, Action<IConsumerConfiguration> configure = null) where T : class;

        /// <summary>
        /// Receive a message from the specified queue. Dispatch them to the given handlers
        /// </summary>
        /// <param name="queue">The queue to take messages from</param>
        /// <param name="addHandlers">A function to add handlers</param>
        /// <param name="configure">Action to configure consumer with</param>
        /// <returns>Consumer cancellation. Call Dispose to stop consuming</returns>
        IDisposable Receive(string queue, Action<IReceiveRegistration> addHandlers, Action<IConsumerConfiguration> configure = null);

    }
}

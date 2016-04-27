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
        IDisposable Receive<T>(string queue, Func<T, Task> onMessage, Action<IConsumerConfiguration> configure = null) where T : class;

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

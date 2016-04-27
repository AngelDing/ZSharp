using System;
using System.Threading.Tasks;

namespace ZSharp.Framework.RabbitMq
{
    public interface IReceiveRegistration
    {
        /// <summary>
        /// Add an asychronous message handler to this receiver
        /// </summary>
        /// <typeparam name="T">The type of message to receive</typeparam>
        /// <param name="onMessage">The message handler</param>
        /// <returns>'this' for fluent configuration</returns>
        IReceiveRegistration Add<T>(Func<T, Task> onMessage) where T : class;

        /// <summary>
        /// Add a message handler to this receiver
        /// </summary>
        /// <typeparam name="T">The type of message to receive</typeparam>
        /// <param name="onMessage">The message handler</param>
        /// <returns>'this' for fluent configuration</returns>
        IReceiveRegistration Add<T>(Action<T> onMessage) where T : class;
    }

    public class HandlerAdder : IReceiveRegistration
    {
        private readonly IHandlerRegistration handlerRegistration;

        public HandlerAdder(IHandlerRegistration handlerRegistration)
        {
            this.handlerRegistration = handlerRegistration;
        }

        public IReceiveRegistration Add<T>(Func<T, Task> onMessage) where T : class
        {
            handlerRegistration.Add<T>((message, info) => onMessage(message.Body));
            return this;
        }

        public IReceiveRegistration Add<T>(Action<T> onMessage) where T : class
        {
            handlerRegistration.Add<T>((message, info) => onMessage(message.Body));
            return this;
        }
    }
}

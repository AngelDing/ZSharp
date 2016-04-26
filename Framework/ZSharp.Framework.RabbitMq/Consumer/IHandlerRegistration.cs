using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZSharp.Framework.Logging;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.RabbitMq
{
    public interface IHandlerRegistration
    {
        /// <summary>
        /// Add an asynchronous handler
        /// </summary>
        /// <typeparam name="T">The message type</typeparam>
        /// <param name="handler">The handler</param>
        /// <returns></returns>
        IHandlerRegistration Add<T>(Func<IMessage<T>, MessageReceivedInfo, Task> handler)
            where T : class;
        
        /// <summary>
        /// Add a synchronous handler
        /// </summary>
        /// <typeparam name="T">The message type</typeparam>
        /// <param name="handler">The handler</param>
        /// <returns></returns>
        IHandlerRegistration Add<T>(Action<IMessage<T>, MessageReceivedInfo> handler)
            where T : class;

        /// <summary>
        /// Set to true if the handler collection should throw an EasyNetQException when no
        /// matching handler is found, or false if it should return a noop handler.
        /// Default is true.
        /// </summary>
        bool ThrowOnNoMatchingHandler { get; set; }
    }

    public interface IHandlerCollection : IHandlerRegistration
    {
        /// <summary>
        /// Retrieve a handler from the collection.
        /// If a matching handler cannot be found, the handler collection will either throw
        /// an EasyNetQException, or return null, depending on the value of the 
        /// ThrowOnNoMatchingHandler property.
        /// </summary>
        /// <typeparam name="T">The type of handler to return</typeparam>
        /// <returns>The handler</returns>
        Func<IMessage<T>, MessageReceivedInfo, Task> GetHandler<T>()
            where T : class;

        /// <summary>
        /// Retrieve a handler from the collection.
        /// If a matching handler cannot be found, the handler collection will either throw
        /// an EasyNetQException, or return null, depending on the value of the 
        /// ThrowOnNoMatchingHandler property.
        /// </summary>
        /// <param name="messageType">The type of handler to return</param>
        /// <returns>The handler</returns>
        Func<IMessage, MessageReceivedInfo, Task> GetHandler(Type messageType);
    }

    public class HandlerCollection : IHandlerCollection
    {
        private readonly IDictionary<Type, Func<IMessage, MessageReceivedInfo, Task>> handlers;

        public HandlerCollection()
        {
            ThrowOnNoMatchingHandler = true;
            handlers = new Dictionary<Type, Func<IMessage, MessageReceivedInfo, Task>>();
        }

        public IHandlerRegistration Add<T>(Func<IMessage<T>, MessageReceivedInfo, Task> handler) where T : class
        {
            GuardHelper.ArgumentNotNull(() => handler);

            if (handlers.ContainsKey(typeof(T)))
            {
                throw new ZRabbitMqException("There is already a handler for message type '{0}'", typeof(T).Name);
            }

            handlers.Add(typeof(T), (iMessage, messageReceivedInfo) => handler((IMessage<T>)iMessage, messageReceivedInfo));
            return this;
        }

        public IHandlerRegistration Add<T>(Action<IMessage<T>, MessageReceivedInfo> handler) where T : class
        {
            Add<T>((message, info) => TaskHelpers.ExecuteSynchronously(() => handler(message, info)));
            return this;
        }

        public Func<IMessage<T>, MessageReceivedInfo, Task> GetHandler<T>() where T : class
        {
            return GetHandler(typeof(T));
        }

        public Func<IMessage, MessageReceivedInfo, Task> GetHandler(Type messageType)
        {
            Func<IMessage, MessageReceivedInfo, Task> func;
            if (handlers.TryGetValue(messageType, out func))
            {
                return func;
            }

            // no exact handler match found, so let's see if we can find a handler that
            // handles a supertype of the consumed message.
            var handlerType = handlers.Keys.FirstOrDefault(type => type.IsAssignableFrom(messageType));
            if (handlerType != null)
            {
                return handlers[handlerType];
            }

            if (ThrowOnNoMatchingHandler)
            {
                var logger = LogManager.GetLogger(GetType());
                logger.Error("No handler found for message type {0}", messageType.Name);
                throw new ZRabbitMqException("No handler found for message type {0}", messageType.Name);
            }

            return (message, info) => Task.Factory.StartNew(() => { });
        }

        public bool ThrowOnNoMatchingHandler { get; set; }
    }
}
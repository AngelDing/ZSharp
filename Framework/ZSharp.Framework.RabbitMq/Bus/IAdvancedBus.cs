using System;
using System.Threading.Tasks;

namespace ZSharp.Framework.RabbitMq
{
    public interface IAdvancedBus : IDisposable
    {
        /// <summary>
        /// Publish a message as a .NET type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exchange">The exchange to publish to</param>
        /// <param name="routingKey">
        /// The routing key for the message. The routing key is used for routing messages 
        /// depending on the exchange configuration.
        /// </param>
        /// <param name="mandatory">
        /// This flag tells the server how to react if the message cannot be routed to a queue. 
        /// If this flag is true, the server will return an unroutable message with a Return method. 
        /// If this flag is false, the server silently drops the message.
        /// </param>
        /// <param name="message">The message to publish</param>
        void Publish<T>(IExchange exchange, string routingKey, bool mandatory, IMessage<T> message) where T : class;

        Task PublishAsync<T>( IExchange exchange, string routingKey,bool mandatory, IMessage<T> message) where T : class;
    }
}
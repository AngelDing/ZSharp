using System;
using System.Threading.Tasks;

namespace ZSharp.Framework.RabbitMq
{
    public interface IAdvancedBus : IDisposable
    {
        /// <summary>
        /// Declare an exchange
        /// </summary>
        /// <param name="name">The exchange name</param>
        /// <param name="type">The type of exchange</param>
        /// <param name="passive">Throw an exception rather than create the exchange if it doens't exist</param>
        /// <param name="durable">Durable exchanges remain active when a server restarts.</param>
        /// <param name="autoDelete">If set, the exchange is deleted when all queues have finished using it.</param>
        /// <param name="internal">If set, the exchange may not be used directly by publishers, but only when bound to other exchanges.</param>
        /// <param name="alternateExchange">Route messages to this exchange if they cannot be routed.</param>
        /// <param name="delayed">If set, declars x-delayed-type exchange for routing delayed messages.</param>
        /// <returns>The exchange</returns>
        IExchange ExchangeDeclare(
            string name,
            string type,
            bool passive = false,
            bool durable = true,
            bool autoDelete = false,
            bool @internal = false,
            string alternateExchange = null,
            bool delayed = false);

        Task<IExchange> ExchangeDeclareAsync(
            string name,
            string type,
            bool passive = false,
            bool durable = true,
            bool autoDelete = false,
            bool @internal = false,
            string alternateExchange = null,
            bool delayed = false);

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
        void Publish<T>(IExchange exchange, string routingKey, bool mandatory, IMessage<T> message)
            where T : class;

        Task PublishAsync<T>( IExchange exchange, string routingKey,bool mandatory, IMessage<T> message)
            where T : class;
    }
}
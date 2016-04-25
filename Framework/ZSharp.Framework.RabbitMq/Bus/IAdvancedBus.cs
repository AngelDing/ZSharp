using System;
using System.Threading.Tasks;

namespace ZSharp.Framework.RabbitMq
{
    public interface IAdvancedBus : IDisposable
    {
        #region Exchange
        
        IExchange ExchangeDeclare(ExchangeDeclareParam param);

        Task<IExchange> ExchangeDeclareAsync(ExchangeDeclareParam param);

        #endregion

        #region Queue
       
        IQueue QueueDeclare(QueueDeclareParam param);

        Task<IQueue> QueueDeclareAsync(QueueDeclareParam param);

        #endregion

        #region Publish

        /// <summary>
        /// Publish a message as a .NET type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exchange">The exchange to publish to</param>
        /// <param name="routingKey">
        /// The routing key for the message. The routing key is used for routing messages 
        /// depending on the exchange configuration.
        /// </param>
        /// <param name="message">The message to publish</param>
        void Publish<T>(IExchange exchange, string routingKey, IMessage<T> message) where T : class;

        Task PublishAsync<T>(IExchange exchange, string routingKey, IMessage<T> message) where T : class;

        #endregion
    }
}
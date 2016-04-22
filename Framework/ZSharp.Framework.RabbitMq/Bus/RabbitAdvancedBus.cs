using System;
using System.Threading.Tasks;

namespace ZSharp.Framework.RabbitMq
{
    public class RabbitAdvancedBus : IAdvancedBus
    {
        public RabbitAdvancedBus()
        {
        }

        #region Exchagne

        public IExchange ExchangeDeclare(
            string name,
            string type,
            bool passive = false,
            bool durable = true,
            bool autoDelete = false,
            bool @internal = false,
            string alternateExchange = null,
            bool delayed = false)
        {
            throw new NotImplementedException();
        }

        public Task<IExchange> ExchangeDeclareAsync(
            string name,
            string type,
            bool passive = false,
            bool durable = true,
            bool autoDelete = false,
            bool @internal = false,
            string alternateExchange = null,
            bool delayed = false)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Publish       

        public void Publish<T>(IExchange exchange, string routingKey, IMessage<T> message)
            where T : class
        {
            new PublishManager<T>(exchange, routingKey, message).Publish();
        }

        public Task PublishAsync<T>(IExchange exchange, string routingKey, IMessage<T> message)
            where T : class
        {
            var manager = new PublishManager<T>(exchange, routingKey, message);
            return manager.PublishAsync();
        }

        #endregion
    }
}

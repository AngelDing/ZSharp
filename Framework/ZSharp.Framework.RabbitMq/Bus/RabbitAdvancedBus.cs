using System;
using System.Threading.Tasks;

namespace ZSharp.Framework.RabbitMq
{
    public class RabbitAdvancedBus : BaseRabbitMq, IAdvancedBus
    {
        public RabbitAdvancedBus(ConnectionConfiguration connectionConfiguration)
            :base(connectionConfiguration)
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

        public void Publish<T>(IExchange exchange, string routingKey, bool mandatory, IMessage<T> message)
            where T : class
        {
            new PublishManager<T>(connectionConfiguration, exchange, routingKey, message).Publish(mandatory);
        }

        public Task PublishAsync<T>(IExchange exchange, string routingKey, bool mandatory, IMessage<T> message)
            where T : class
        {
            var manager = new PublishManager<T>(connectionConfiguration, exchange, routingKey, message);
            return manager.PublishAsync(mandatory);
        }     

        #endregion
    }
}

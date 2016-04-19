using System;
using System.Threading.Tasks;

namespace ZSharp.Framework.RabbitMq
{
    public class RabbitAdvancedBus : DisposableObject, IAdvancedBus
    {
        public void Publish<T>(IExchange exchange, string routingKey, bool mandatory, IMessage<T> message)
            where T : class
        {
            throw new NotImplementedException();
        }

        public Task PublishAsync<T>(IExchange exchange, string routingKey, bool mandatory, IMessage<T> message)
            where T : class
        {
            throw new NotImplementedException();
        }

        protected override void Dispose(bool disposing)
        {
            throw new NotImplementedException();
        }
    }
}

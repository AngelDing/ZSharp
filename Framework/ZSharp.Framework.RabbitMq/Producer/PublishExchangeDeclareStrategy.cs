using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ZSharp.Framework.RabbitMq
{
    public class PublishExchangeDeclareStrategy : BaseRabbitMq, IPublishExchangeDeclareStrategy
    {
        private readonly ConcurrentDictionary<string, IExchange> exchanges = new ConcurrentDictionary<string, IExchange>();
        private readonly AsyncSemaphore semaphore = new AsyncSemaphore(1);

        public IExchange DeclareExchange(IAdvancedBus advancedBus, string exchangeName, string exchangeType)
        {
            IExchange exchange;
            if (exchanges.TryGetValue(exchangeName, out exchange))
            {
                return exchange;
            }
            semaphore.Wait();
            try
            {
                if (exchanges.TryGetValue(exchangeName, out exchange))
                {
                    return exchange;
                }
                var param = new ExchangeDeclareParam(exchangeName, exchangeType);
                exchange = advancedBus.ExchangeDeclare(param);
                exchanges[exchangeName] = exchange;
                return exchange;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public IExchange DeclareExchange(IAdvancedBus advancedBus, Type messageType, string exchangeType)
        {
            var exchangeName = Conventions.ExchangeNamingConvention(messageType);
            return DeclareExchange(advancedBus, exchangeName, exchangeType);
        }

        public async Task<IExchange> DeclareExchangeAsync(IAdvancedBus advancedBus, string exchangeName, string exchangeType)
        {
            IExchange exchange;
            if (exchanges.TryGetValue(exchangeName, out exchange))
            {
                return exchange;
            }
            await semaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                if (exchanges.TryGetValue(exchangeName, out exchange))
                {
                    return exchange;
                }
                var param = new ExchangeDeclareParam(exchangeName, exchangeType);
                exchange = await advancedBus.ExchangeDeclareAsync(param).ConfigureAwait(false);
                exchanges[exchangeName] = exchange;
                return exchange;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public Task<IExchange> DeclareExchangeAsync(IAdvancedBus advancedBus, Type messageType, string exchangeType)
        {
            var exchangeName = Conventions.ExchangeNamingConvention(messageType);
            return DeclareExchangeAsync(advancedBus, exchangeName, exchangeType);
        }
    }
}

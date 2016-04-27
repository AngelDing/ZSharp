using System;
using System.Linq;
using System.Threading.Tasks;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.RabbitMq
{
    public class SubscriberManager : BaseRabbitMq
    {
        private readonly IAdvancedBus advancedBus;
        private Action<ISubscriptionConfiguration> configure;
        private SubscriptionConfiguration subConfigure;

        public SubscriberManager(IAdvancedBus advancedBus, Action<ISubscriptionConfiguration> configure)
        {
            this.advancedBus = advancedBus;
            this.configure = configure;
            subConfigure = new SubscriptionConfiguration(RabbitMqConfiguration.PrefetchCount);
            if (configure != null)
            {
                configure(subConfigure);
            }
        }

        public ISubscriptionResult SubscribeAsync<T>(string subscriptionId, Func<T, Task> onMessage)
            where T : class
        {
            GuardHelper.ArgumentNotEmpty(() => subscriptionId);
            GuardHelper.ArgumentNotNull(() => onMessage);

            var queueName = Conventions.QueueNamingConvention(typeof(T), subscriptionId);
            var exchangeName = Conventions.ExchangeNamingConvention(typeof(T));
            IQueue queue = QueueDeclare(queueName);
            var exchange = ExchangeDeclare(exchangeName);
            foreach (var topic in subConfigure.Topics.DefaultIfEmpty("#"))
            {
                advancedBus.Bind(exchange, queue, topic);
            }

            var consumerCancellation = advancedBus.Consume<T>(
                queue,
                (message, messageReceivedInfo) => onMessage(message.Body),
                x => { InitIConsumerConfiguration(x); });
            return new SubscriptionResult(exchange, queue, consumerCancellation);
        }

        private IExchange ExchangeDeclare(string exchangeName)
        {
            var parma = new ExchangeDeclareParam(exchangeName, ExchangeType.Topic);
            return advancedBus.ExchangeDeclare(parma);
        }

        private void InitIConsumerConfiguration(IConsumerConfiguration consumerConfigure)
        {
            consumerConfigure
                .WithPriority(subConfigure.Priority)
                .WithCancelOnHaFailover(subConfigure.CancelOnHaFailover)
                .WithPrefetchCount(subConfigure.PrefetchCount);
            if (subConfigure.IsExclusive)
            {
                consumerConfigure.AsExclusive();
            }
        }

        private IQueue QueueDeclare(string queueName)
        {
            var param = new QueueDeclareParam(queueName);
            param.AutoDelete = subConfigure.AutoDelete;
            param.Expires = subConfigure.Expires;
            return advancedBus.QueueDeclare(param);
        }
    }
}

using System.Threading.Tasks;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.RabbitMq
{
    public class RabbitBus : DisposableObject, IBus
    {
        private readonly IConventions conventions;
        private readonly IAdvancedBus advancedBus;
        private readonly IPublishExchangeDeclareStrategy publishExchangeDeclareStrategy;
        private readonly IMessageDeliveryModeStrategy messageDeliveryModeStrategy;

        public RabbitBus(
            IConventions conventions,
            IAdvancedBus advancedBus,
            IPublishExchangeDeclareStrategy publishExchangeDeclareStrategy,
            IMessageDeliveryModeStrategy messageDeliveryModeStrategy)
        {
            this.conventions = conventions;
            this.advancedBus = advancedBus;
            this.publishExchangeDeclareStrategy = publishExchangeDeclareStrategy;
            this.messageDeliveryModeStrategy = messageDeliveryModeStrategy;
        }

        public void Publish<T>(T message) where T : class
        {
            Publish(message, conventions.TopicNamingConvention(typeof(T)));
        }

        public void Publish<T>(T message, string topic) where T : class
        {
            GuardHelper.ArgumentNotNull(() => message);
            GuardHelper.ArgumentNotNull(() => topic);

            var messageType = typeof(T);
            var rabbitMessage = new Message<T>(message)
            {
                Properties =
                {
                    DeliveryMode = messageDeliveryModeStrategy.GetDeliveryMode(messageType)
                }
            };
            var exchange = publishExchangeDeclareStrategy.DeclareExchange(
                advancedBus, messageType, ExchangeType.Topic);
            advancedBus.Publish(exchange, topic, false, rabbitMessage);
        }

        public Task PublishAsync<T>(T message) where T : class
        {
            return PublishAsync(message, conventions.TopicNamingConvention(typeof(T)));
        }

        public async Task PublishAsync<T>(T message, string topic) where T : class
        {
            GuardHelper.ArgumentNotNull(() => message);
            GuardHelper.ArgumentNotNull(() => topic);
            var messageType = typeof(T);
            var rabbitMessage = new Message<T>(message)
            {
                Properties =
                {
                    DeliveryMode = messageDeliveryModeStrategy.GetDeliveryMode(messageType)
                }
            };
            var exchange = await publishExchangeDeclareStrategy.DeclareExchangeAsync(
                advancedBus, messageType, ExchangeType.Topic).ConfigureAwait(false);
            await advancedBus.PublishAsync(exchange, topic, false, rabbitMessage).ConfigureAwait(false);
        }

        protected override void Dispose(bool disposing)
        {
            //if (disposing)
            //{
            //    advancedBus.Dispose();
            //}
        }
    }
}

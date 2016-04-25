using System;
using System.Threading.Tasks;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.RabbitMq
{
    public class RabbitBus : DisposableObject, IBus
    {
        private readonly IConventions conventions;
        private readonly IAdvancedBus advancedBus;
        private readonly ISendReceive sendReceive;
        private readonly IPublishExchangeDeclareStrategy publishExchangeDeclareStrategy;
        private readonly IMessageDeliveryModeStrategy messageDeliveryModeStrategy;

        public RabbitBus(
            IConventions conventions,
            IAdvancedBus advancedBus,
            ISendReceive sendReceive,
            IPublishExchangeDeclareStrategy publishExchangeDeclareStrategy,
            IMessageDeliveryModeStrategy messageDeliveryModeStrategy)
        {
            this.conventions = conventions;
            this.advancedBus = advancedBus;
            this.sendReceive = sendReceive;
            this.publishExchangeDeclareStrategy = publishExchangeDeclareStrategy;
            this.messageDeliveryModeStrategy = messageDeliveryModeStrategy;
        }

        #region Pub/Sub

        public void Publish<T>(T message) where T : class
        {
            Publish(message, conventions.TopicNamingConvention(typeof(T)));
        }

        public void Publish<T>(T message, string topic) where T : class
        {
            GuardHelper.ArgumentNotNull(() => message);

            var messageType = typeof(T);
            Message<T> rabbitMessage = GetRabbitMessage(message, messageType);
            var exchange = publishExchangeDeclareStrategy.DeclareExchange(
                advancedBus, messageType, ExchangeType.Topic);
            advancedBus.Publish(exchange, topic, rabbitMessage);
        }       

        public Task PublishAsync<T>(T message) where T : class
        {
            return PublishAsync(message, conventions.TopicNamingConvention(typeof(T)));
        }

        public async Task PublishAsync<T>(T message, string topic) where T : class
        {
            GuardHelper.ArgumentNotNull(() => message);
            GuardHelper.ArgumentNotEmpty(() => topic);
            var messageType = typeof(T);
            var rabbitMessage = GetRabbitMessage(message, messageType);
            var exchange = await publishExchangeDeclareStrategy.DeclareExchangeAsync(
                advancedBus, messageType, ExchangeType.Topic).ConfigureAwait(false);
            await advancedBus.PublishAsync(exchange, topic, rabbitMessage).ConfigureAwait(false);
        }

        private Message<T> GetRabbitMessage<T>(T message, Type messageType) where T : class
        {
            return new Message<T>(message)
            {
                Properties =
                {
                    DeliveryMode = messageDeliveryModeStrategy.GetDeliveryMode(messageType)
                }
            };
        }

        #endregion

        #region Send/Receive
        public void Send<T>(string queue, T message) where T : class
        {
            sendReceive.Send(queue, message);
        }

        public Task SendAsync<T>(string queue, T message) where T : class
        {
            return sendReceive.SendAsync(queue, message);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                advancedBus.Dispose();
            }
        }
    }
}

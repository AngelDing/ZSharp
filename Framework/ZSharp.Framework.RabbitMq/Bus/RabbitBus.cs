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

        #region Pub

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

        #region Sub

        public ISubscriptionResult Subscribe<T>(
            string subscriptionId, 
            Action<T> onMessage, 
            Action<ISubscriptionConfiguration> configure = null)
           where T : class
        {
            return SubscribeAsync<T>(
                subscriptionId, 
                msg => TaskHelpers.ExecuteSynchronously(() => onMessage(msg)),
                configure);
        }

        public ISubscriptionResult SubscribeAsync<T>(
            string subscriptionId,
            Func<T, Task> onMessage,
            Action<ISubscriptionConfiguration> configure = null)
            where T : class
        {
            var manager = new SubscriberManager(advancedBus, configure);
            return manager.SubscribeAsync(subscriptionId, onMessage);
        }

        #endregion

        #region Send

        public void Send<T>(string queue, T message) where T : class
        {
            sendReceive.Send(queue, message);
        }

        public Task SendAsync<T>(string queue, T message) where T : class
        {
            return sendReceive.SendAsync(queue, message);
        }

        #endregion

        #region Receive

        public IDisposable Receive<T>(string queue, Action<T> onMessage, Action<IConsumerConfiguration> configure = null) where T : class
        {
            return ReceiveAsync<T>(queue, message => TaskHelpers.ExecuteSynchronously(() => onMessage(message)), configure);
        }

        public IDisposable ReceiveAsync<T>(string queue, Func<T, Task> onMessage, Action<IConsumerConfiguration> configure = null) where T : class
        {
            return sendReceive.Receive(queue, onMessage, configure);
        }

        public IDisposable Receive(string queue, Action<IReceiveRegistration> addHandlers, Action<IConsumerConfiguration> configure = null)
        {
            return sendReceive.Receive(queue, addHandlers, configure);
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

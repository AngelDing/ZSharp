using System;
using ZSharp.Framework.Utils;
using ZSharp.Framework.Infrastructure;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace ZSharp.Framework.RabbitMq
{
    public class PublishManager<T> : BaseRabbitMq where T: class
    {
        private readonly IPublishConfirmationListener confirmationListener;
        private readonly IExchange exchange;
        private readonly string routingKey;
        private bool mandatory;
        private MessageProperties msgProperties;
        private byte[] body;

        private RawMessage rawMsg;
        private RawMessage RawMessage
        {
            get
            {
                if (rawMsg == null)
                {
                    var produceConsumeInterceptor = ServiceLocator.GetInstance<IProduceConsumeInterceptor>();
                    rawMsg = produceConsumeInterceptor.OnProduce(new RawMessage(msgProperties, body));
                }
                return rawMsg;
            }
        }

        public PublishManager(IExchange exchange, string routingKey, IMessage<T> message)
        {
            GuardHelper.ArgumentNotNull(() => exchange);
            GuardHelper.ArgumentNotNull(() => routingKey);
            GuardHelper.ArgumentNotNull(() => message);
            this.exchange = exchange;
            this.routingKey = routingKey;
            mandatory = RabbitMqConfiguration.Mandatory;
            SerializeMessage(message);
            confirmationListener = ServiceLocator.GetInstance<IPublishConfirmationListener>();
        }

        private void SerializeMessage(IMessage<T> message)
        {
            var serializationStrategy = ServiceLocator.GetInstance<IMessageSerializationStrategy>();
            var serializedMessage = serializationStrategy.SerializeMessage(message);
            msgProperties = serializedMessage.Properties;
            body = serializedMessage.Body;
        }

        public void Publish()
        {
            if (RabbitMqConfiguration.PublisherConfirms)
            {
                var timeBudget = new TimeBudget(TimeSpan.FromSeconds(RabbitMqConfiguration.Timeout)).Start();
                while (!timeBudget.IsExpired())
                {
                    var confirmsWaiter = clientCommandDispatcher.Invoke(model =>
                    {
                        return GetConfirmationWaiter(model);
                    });
                    try
                    {
                        confirmsWaiter.Wait(timeBudget.GetRemainingTime());
                        break;
                    }
                    catch (PublishInterruptedException ex)
                    {
                        Logger.Error("Publish Interrupted Exception", ex);
                    }
                }
            }
            else
            {
                clientCommandDispatcher.Invoke(model => { BasicPublish(model); });
            }

            InternalEventPublishAndLog();
        }        

        public async Task PublishAsync()
        { 
            if (RabbitMqConfiguration.PublisherConfirms)
            {
                var timeBudget = new TimeBudget(TimeSpan.FromSeconds(RabbitMqConfiguration.Timeout)).Start();
                while (!timeBudget.IsExpired())
                {
                    var confirmsWaiter = await clientCommandDispatcher.InvokeAsync(model =>
                    {
                        return GetConfirmationWaiter(model);
                    }).ConfigureAwait(false);
                    try
                    {
                        await confirmsWaiter.WaitAsync(timeBudget.GetRemainingTime()).ConfigureAwait(false);
                        break;
                    }
                    catch (PublishInterruptedException ex)
                    {
                        Logger.Error("Publish Interrupted Exception", ex);
                    }
                }
            }
            else
            {
                await clientCommandDispatcher.InvokeAsync(model => { BasicPublish(model); }).ConfigureAwait(false);
            }
            InternalEventPublishAndLog();
        }

        private void InternalEventPublishAndLog()
        {
            var publishedMsg = new PublishedMessageEvent(
                exchange.Name, routingKey, RawMessage.Properties, RawMessage.Body);
            eventBus.Publish(publishedMsg);

            var msgTemplate = "Published to exchange: '{0}', routing key: '{1}', correlationId: '{2}'";
            Logger.Debug(msgTemplate, exchange.Name, routingKey, msgProperties.CorrelationId);
        }

        private IPublishConfirmationWaiter GetConfirmationWaiter(IModel model)
        {
            var waiter = confirmationListener.GetWaiter(model);                  
            try
            {
                BasicPublish(model);
            }
            catch (Exception ex)
            {
                waiter.Cancel();
                Logger.Error("Basic Publish Exception", ex);
                throw;
            }
            return waiter;
        }

        private void BasicPublish(IModel model)
        {
            var properties = model.CreateBasicProperties();
            RawMessage.Properties.CopyTo(properties);
            model.BasicPublish(exchange.Name, routingKey, mandatory, properties, RawMessage.Body);
        }

        protected override void Dispose(bool disposing)
        {
            throw new NotImplementedException();
        }
    }
}

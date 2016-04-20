using System;
using ZSharp.Framework.Utils;
using ZSharp.Framework.Infrastructure;

namespace ZSharp.Framework.RabbitMq
{
    public class PublishManager<T> : BaseRabbitMq where T: class
    {
        private readonly IPublishConfirmationListener confirmationListener;
        private readonly IExchange exchange;
        private readonly string routingKey;
        private MessageProperties msgProperties;
        private byte[] body;

        public PublishManager(
            ConnectionConfiguration connectionConfiguration, 
            IExchange exchange, string routingKey, IMessage<T> message)
            : base(connectionConfiguration)
        {
            GuardHelper.ArgumentNotNull(() => exchange);
            GuardHelper.ArgumentNotNull(() => routingKey);
            GuardHelper.ArgumentNotNull(() => message);
            this.exchange = exchange;
            this.routingKey = routingKey;
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

        public void Publish(bool mandatory)
        {
            var rawMessage = produceConsumeInterceptor.OnProduce(new RawMessage(msgProperties, body));
            if (connectionConfiguration.PublisherConfirms)
            {
                var timeBudget = new TimeBudget(TimeSpan.FromSeconds(connectionConfiguration.Timeout)).Start();
                while (!timeBudget.IsExpired())
                {
                    var confirmsWaiter = clientCommandDispatcher.Invoke(model =>
                    {
                        var properties = model.CreateBasicProperties();
                        rawMessage.Properties.CopyTo(properties);

                        var waiter = confirmationListener.GetWaiter(model);

                        try
                        {
                            model.BasicPublish(exchange.Name, routingKey, mandatory, properties, rawMessage.Body);
                        }
                        catch (Exception)
                        {
                            waiter.Cancel();
                            throw;
                        }

                        return waiter;
                    });

                    try
                    {
                        confirmsWaiter.Wait(timeBudget.GetRemainingTime());
                        break;
                    }
                    catch (PublishInterruptedException)
                    {
                    }
                }
            }
            else
            {
                clientCommandDispatcher.Invoke(model =>
                {
                    var properties = model.CreateBasicProperties();
                    rawMessage.Properties.CopyTo(properties);
                    model.BasicPublish(exchange.Name, routingKey, mandatory, properties, rawMessage.Body);
                });
            }
            eventBus.Publish(new PublishedMessageEvent(exchange.Name, routingKey, rawMessage.Properties, rawMessage.Body));
            Logger.Debug("Published to exchange: '{0}', routing key: '{1}', correlationId: '{2}'",
                exchange.Name, routingKey, msgProperties.CorrelationId);
        }

        //public virtual async Task PublishAsync(
        //   IExchange exchange,
        //   string routingKey,
        //   bool mandatory,
        //   MessageProperties messageProperties,
        //   byte[] body)
        //{
        //    //Preconditions.CheckNotNull(exchange, "exchange");
        //    //Preconditions.CheckShortString(routingKey, "routingKey");
        //    //Preconditions.CheckNotNull(messageProperties, "messageProperties");
        //    //Preconditions.CheckNotNull(body, "body");

        //    //// Fix me: It's very hard now to move publish logic to separate abstraction, just leave it here. 
        //    //var rawMessage = produceConsumeInterceptor.OnProduce(new RawMessage(messageProperties, body));
        //    //if (connectionConfiguration.PublisherConfirms)
        //    //{
        //    //    var timeBudget = new TimeBudget(TimeSpan.FromSeconds(connectionConfiguration.Timeout)).Start();
        //    //    while (!timeBudget.IsExpired())
        //    //    {
        //    //        var confirmsWaiter = await clientCommandDispatcher.InvokeAsync(model =>
        //    //        {
        //    //            var properties = model.CreateBasicProperties();
        //    //            rawMessage.Properties.CopyTo(properties);
        //    //            var waiter = confirmationListener.GetWaiter(model);

        //    //            try
        //    //            {
        //    //                model.BasicPublish(exchange.Name, routingKey, mandatory, properties, rawMessage.Body);
        //    //            }
        //    //            catch (Exception)
        //    //            {
        //    //                waiter.Cancel();
        //    //                throw;
        //    //            }

        //    //            return waiter;
        //    //        }).ConfigureAwait(false);

        //    //        try
        //    //        {
        //    //            await confirmsWaiter.WaitAsync(timeBudget.GetRemainingTime()).ConfigureAwait(false);
        //    //            break;
        //    //        }
        //    //        catch (PublishInterruptedException)
        //    //        {
        //    //        }
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    await clientCommandDispatcher.InvokeAsync(model =>
        //    //    {
        //    //        var properties = model.CreateBasicProperties();
        //    //        rawMessage.Properties.CopyTo(properties);
        //    //        model.BasicPublish(exchange.Name, routingKey, mandatory, properties, rawMessage.Body);
        //    //    }).ConfigureAwait(false);
        //    //}
        //    //eventBus.Publish(new PublishedMessageEvent(exchange.Name, routingKey, rawMessage.Properties, rawMessage.Body));
        //    //logger.DebugWrite("Published to exchange: '{0}', routing key: '{1}', correlationId: '{2}'", exchange.Name, routingKey, messageProperties.CorrelationId);
        //}
    }
}

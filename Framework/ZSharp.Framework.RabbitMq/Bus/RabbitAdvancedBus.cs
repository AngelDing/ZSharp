using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZSharp.Framework.Infrastructure;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.RabbitMq
{
    public class RabbitAdvancedBus : BaseRabbitMq, IAdvancedBus
    {
        private readonly IPersistentConnection connection;
        private readonly IClientCommandDispatcher clientCommandDispatcher;
        private readonly IPublishConfirmationListener confirmationListener;

        public RabbitAdvancedBus(
            IConnectionFactory connectionFactory,
            IClientCommandDispatcherFactory clientCommandDispatcherFactory,
            IPublishConfirmationListener confirmationListener)
        {
            this.confirmationListener = confirmationListener;
            connection = new PersistentConnection(connectionFactory);
            clientCommandDispatcher = clientCommandDispatcherFactory.GetClientCommandDispatcher(connection);
            connection.Initialize();
        }

        #region Exchagne

        public IExchange ExchangeDeclare(ExchangeDeclareParam param)
        {
            var name = param.Name;    
            if (param.Passive)
            {
                clientCommandDispatcher.Invoke(x => x.ExchangeDeclarePassive(name));
                return new Exchange(name);
            }
            var arguments = GetExchangeArguments(param);
            clientCommandDispatcher.Invoke(x =>
            {
                x.ExchangeDeclare(name, param.Type, param.Durable, param.AutoDelete, arguments);
            });
            LogExchangeDeclare(param);
            return new Exchange(name);
        }       

        public async Task<IExchange> ExchangeDeclareAsync(ExchangeDeclareParam param)
        {
            var name = param.Name;
            if (param.Passive)
            {
                await clientCommandDispatcher
                    .InvokeAsync(x => x.ExchangeDeclarePassive(name))
                    .ConfigureAwait(false);
                return new Exchange(name);
            }
            var arguments = GetExchangeArguments(param);
            await clientCommandDispatcher
                .InvokeAsync(x => x.ExchangeDeclare(name, param.Type, param.Durable, param.AutoDelete, arguments))
                .ConfigureAwait(false);
            LogExchangeDeclare(param);
            return new Exchange(name);
        }

        private void LogExchangeDeclare(ExchangeDeclareParam param)
        {
            var msgTemplate = "Declared Exchange: {0} type:{1}, durable:{2}, autoDelete:{3}, delayed:{4}";
            Logger.Debug(msgTemplate, param.Name, param.Type, param.Durable, param.AutoDelete, param.Delayed);
        }

        private IDictionary<string, object> GetExchangeArguments(ExchangeDeclareParam param)
        {
            IDictionary<string, object> arguments = new Dictionary<string, object>();
            if (param.AlternateExchange != null)
            {
                arguments.Add("alternate-exchange", param.AlternateExchange);
            }
            if (param.Delayed)
            {
                arguments.Add("x-delayed-type", param.Type);
                param.Type = "x-delayed-message";
            }
            return arguments;
        }

        #endregion

        #region Queue

        public IQueue QueueDeclare(QueueDeclareParam param)
        {
            var name = param.Name;
            var queue = new Queue(name, param.Exclusive);
            var arguments = new Dictionary<string, object>();
            if (param.Passive)
            {
                clientCommandDispatcher.Invoke(x => x.QueueDeclarePassive(name));
            }
            else
            {
                arguments = GetQueueDeclareArguments(param);
                clientCommandDispatcher.Invoke(
                    x => x.QueueDeclare(name, param.Durable, param.Exclusive, param.AutoDelete, arguments));
            }
            LogQueueDeclare(param, arguments);
            return queue;
        }

        public async Task<IQueue> QueueDeclareAsync(QueueDeclareParam param)
        {
            var name = param.Name;
            var queue = new Queue(name, param.Exclusive);
            var arguments = new Dictionary<string, object>();
            if (param.Passive)
            {
                await clientCommandDispatcher.InvokeAsync(x => x.QueueDeclarePassive(name)).ConfigureAwait(false);
            }
            else
            {
                arguments = GetQueueDeclareArguments(param);
                await clientCommandDispatcher.InvokeAsync(
                    x => x.QueueDeclare(name, param.Durable, param.Exclusive, param.AutoDelete, arguments))
                    .ConfigureAwait(false);
            }
            LogQueueDeclare(param, arguments);
            return queue;
        }

        private void LogQueueDeclare(QueueDeclareParam param, Dictionary<string, object> arguments)
        {
            var msgTemplate = "Declared Queue: '{0}', durable:{1}, exclusive:{2}, autoDelete:{3}, args:{4}";
            var args = string.Join(", ", arguments.Select(kvp => string.Format("{0}={1}", kvp.Key, kvp.Value)));
            Logger.Debug(msgTemplate, param.Name, param.Durable, param.Exclusive, param.AutoDelete, args);
        }

        private Dictionary<string, object> GetQueueDeclareArguments(QueueDeclareParam param)
        {
            var arguments = new Dictionary<string, object>();
            if (param.PerQueueMessageTtl.HasValue)
            {
                arguments.Add("x-message-ttl", param.PerQueueMessageTtl.Value);
            }
            if (param.Expires.HasValue)
            {
                arguments.Add("x-expires", param.Expires);
            }
            if (param.MaxPriority.HasValue)
            {
                arguments.Add("x-max-priority", param.MaxPriority.Value);
            }
            if (!string.IsNullOrEmpty(param.DeadLetterExchange))
            {
                arguments.Add("x-dead-letter-exchange", param.DeadLetterExchange);
            }
            if (!string.IsNullOrEmpty(param.DeadLetterRoutingKey))
            {
                arguments.Add("x-dead-letter-routing-key", param.DeadLetterRoutingKey);
            }
            if (param.MaxLength.HasValue)
            {
                arguments.Add("x-max-length", param.MaxLength.Value);
            }
            if (param.MaxLengthBytes.HasValue)
            {
                arguments.Add("x-max-length-bytes", param.MaxLengthBytes.Value);
            }

            return arguments;
        }

        #endregion

        #region Bind

        public IBinding Bind(IExchange exchange, IQueue queue, string routingKey)
        {
            clientCommandDispatcher.Invoke(x => x.QueueBind(queue.Name, exchange.Name, routingKey));
            Logger.Debug("Bound queue {0} to exchange {1} with routing key {2}", queue.Name, exchange.Name, routingKey);
            return new Binding(queue, exchange, routingKey);
        }

        #endregion

        #region Publish       

        public void Publish<T>(IExchange exchange, string routingKey, IMessage<T> message)
            where T : class
        {
            new PublisherManager<T>(clientCommandDispatcher, confirmationListener)
                .Publish(exchange, routingKey, message);
        }

        public Task PublishAsync<T>(IExchange exchange, string routingKey, IMessage<T> message)
            where T : class
        {
            var manager = new PublisherManager<T>(clientCommandDispatcher, confirmationListener);
            return manager.PublishAsync(exchange, routingKey, message);
        }

        #endregion

        #region Consume

        public IDisposable Consume<T>(
            IQueue queue, 
            Func<IMessage<T>, MessageReceivedInfo, Task> onMessage,
            Action<IConsumerConfiguration> configure = null) 
            where T : class
        {
            return Consume(queue, x => x.Add(onMessage), configure);
        }

        public IDisposable Consume(
            IQueue queue, 
            Action<IHandlerRegistration> addHandlers,
            Action<IConsumerConfiguration> configure = null)
        {
            var handlerCollection = new HandlerCollection();
            addHandlers(handlerCollection);
            return Consume(queue, (body, properties, messageReceivedInfo) =>
            {
                var deserializedMessage = SerializationStrategy.DeserializeMessage(properties, body);
                var handler = handlerCollection.GetHandler(deserializedMessage.MessageType);
                return handler(deserializedMessage, messageReceivedInfo);
            }, configure);
        }

        private IDisposable Consume(
            IQueue queue, 
            Func<byte[], MessageProperties, MessageReceivedInfo, Task> onMessage, 
            Action<IConsumerConfiguration> configure)
        {
            GuardHelper.ArgumentNotNull(() => queue);
            GuardHelper.ArgumentNotNull(() => onMessage);
            if (disposed)
            {
                throw new ZRabbitMqException("This bus has been disposed");
            }

            var consumerConfiguration = new ConsumerConfiguration(RabbitMqConfiguration.PrefetchCount);
            if (configure != null)
            {
                configure(consumerConfiguration);
            }

            var consumerFactory = ServiceLocator.GetInstance<IConsumerFactory>();
            var consumer = consumerFactory.CreateConsumer(queue, (body, properties, receviedInfo) =>
            {
                var rawMessage = ProduceConsumeInterceptor.OnConsume(new RawMessage(properties, body));
                return onMessage(rawMessage.Body, rawMessage.Properties, receviedInfo);
            }, connection, consumerConfiguration);
            return consumer.StartConsuming();
        }

        #endregion

        private bool disposed = false;
        public virtual void Dispose()
        {
            if (disposed)
            {
                return;
            }
            //consumerFactory.Dispose();
            confirmationListener.Dispose();
            clientCommandDispatcher.Dispose();
            //connection.Dispose();

            disposed = true;
            Logger.Debug("Connection disposed");
        }
    }
}

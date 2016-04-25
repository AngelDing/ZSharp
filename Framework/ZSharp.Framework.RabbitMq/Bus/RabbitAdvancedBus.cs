using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            throw new NotImplementedException();
        }

        public Task<IQueue> QueueDeclareAsync(QueueDeclareParam param)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Publish       

        public void Publish<T>(IExchange exchange, string routingKey, IMessage<T> message)
            where T : class
        {
            new PublishManager<T>(clientCommandDispatcher, confirmationListener)
                .Publish(exchange, routingKey, message);
        }

        public Task PublishAsync<T>(IExchange exchange, string routingKey, IMessage<T> message)
            where T : class
        {
            var manager = new PublishManager<T>(clientCommandDispatcher, confirmationListener);
            return manager.PublishAsync(exchange, routingKey, message);
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

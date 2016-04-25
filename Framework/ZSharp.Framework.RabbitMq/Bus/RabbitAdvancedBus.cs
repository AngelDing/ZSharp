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

        public IExchange ExchangeDeclare(
            string name,
            string type,
            bool passive = false,
            bool durable = true,
            bool autoDelete = false,
            bool @internal = false,
            string alternateExchange = null,
            bool delayed = false)
        {
            GuardHelper.CheckStringLength(() => name, 255);
            GuardHelper.CheckStringLength(() => type, 255);
            if (passive)
            {
                clientCommandDispatcher.Invoke(x => x.ExchangeDeclarePassive(name));
                return new Exchange(name);
            }
            var arguments = GetExchangeArguments(ref type, alternateExchange, delayed);
            clientCommandDispatcher.Invoke(x =>
            {
                x.ExchangeDeclare(name, type, durable, autoDelete, arguments);
            });
            LogExchangeDeclare(name, type, durable, autoDelete, delayed);
            return new Exchange(name);
        }       

        public async Task<IExchange> ExchangeDeclareAsync(
            string name,
            string type,
            bool passive = false,
            bool durable = true,
            bool autoDelete = false,
            bool @internal = false,
            string alternateExchange = null,
            bool delayed = false)
        {
            GuardHelper.CheckStringLength(() => name, 255);
            GuardHelper.CheckStringLength(() => type, 255);

            if (passive)
            {
                await clientCommandDispatcher
                    .InvokeAsync(x => x.ExchangeDeclarePassive(name))
                    .ConfigureAwait(false);
                return new Exchange(name);
            }
            var arguments = GetExchangeArguments(ref type, alternateExchange, delayed);
            await clientCommandDispatcher
                .InvokeAsync(x => x.ExchangeDeclare(name, type, durable, autoDelete, arguments))
                .ConfigureAwait(false);
            LogExchangeDeclare(name, type, durable, autoDelete, delayed);
            return new Exchange(name);
        }

        private void LogExchangeDeclare(string name, string type, bool durable, bool autoDelete, bool delayed)
        {
            var msgTemplate = "Declared Exchange: {0} type:{1}, durable:{2}, autoDelete:{3}, delayed:{4}";
            Logger.Debug(msgTemplate, name, type, durable, autoDelete, delayed);
        }

        private IDictionary<string, object> GetExchangeArguments(
            ref string type, string alternateExchange, bool delayed)
        {
            IDictionary<string, object> arguments = new Dictionary<string, object>();
            if (alternateExchange != null)
            {
                arguments.Add("alternate-exchange", alternateExchange);
            }
            if (delayed)
            {
                arguments.Add("x-delayed-type", type);
                type = "x-delayed-message";
            }
            return arguments;
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

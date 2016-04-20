using ZSharp.Framework.Logging;

namespace ZSharp.Framework.RabbitMq
{
    public abstract class BaseRabbitMq : DisposableObject
    {
        protected ILogger Logger{ get; private set; }        
        
        protected IEventBus eventBus { get; set; }

        protected IClientCommandDispatcher clientCommandDispatcher { get; set; }

        protected readonly ConnectionConfiguration connectionConfiguration;

        public BaseRabbitMq(ConnectionConfiguration connectionConfiguration)
        {
            Logger = LogManager.GetLogger(GetType());
            this.connectionConfiguration = connectionConfiguration;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //consumerFactory.Dispose();
                //confirmationListener.Dispose();
                clientCommandDispatcher.Dispose();
                //connection.Dispose();

                Logger.Debug("Connection disposed");
            }
        }
    }
}

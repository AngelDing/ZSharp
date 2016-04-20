
using ZSharp.Framework.Logging;

namespace ZSharp.Framework.RabbitMq
{
    public abstract class BaseRabbitMq
    {
        protected ILogger Logger{ get; private set; }

        protected IProduceConsumeInterceptor produceConsumeInterceptor { get; set; }
        
        protected IEventBus eventBus { get; set; }

        protected IClientCommandDispatcher clientCommandDispatcher { get; set; }

        protected readonly ConnectionConfiguration connectionConfiguration;

        public BaseRabbitMq(ConnectionConfiguration connectionConfiguration)
        {
            Logger = LogManager.GetLogger(GetType());
            this.connectionConfiguration = connectionConfiguration;
        }
    }
}

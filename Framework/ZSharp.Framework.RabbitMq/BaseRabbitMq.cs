using ZSharp.Framework.Configurations;
using ZSharp.Framework.Logging;
using ZSharp.Framework.Infrastructure;

namespace ZSharp.Framework.RabbitMq
{
    public abstract class BaseRabbitMq : DisposableObject
    {
        protected ILogger Logger{ get; private set; }        
        
        protected IEventBus eventBus { get; set; }

        protected IClientCommandDispatcher clientCommandDispatcher { get; set; }

        private IRabbitMqConfiguration rabbitMqConfiguration;
        protected IRabbitMqConfiguration RabbitMqConfiguration
        {
            get
            {
                if (rabbitMqConfiguration == null)
                {
                    rabbitMqConfiguration = ServiceLocator.GetInstance<IRabbitMqConfiguration>();
                }
                return rabbitMqConfiguration;
            }
        }

        public BaseRabbitMq()
        {
            Logger = LogManager.GetLogger(GetType());
        }
    }
}

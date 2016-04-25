using ZSharp.Framework.Configurations;
using ZSharp.Framework.Logging;
using ZSharp.Framework.Infrastructure;

namespace ZSharp.Framework.RabbitMq
{
    public abstract class BaseRabbitMq
    {
        protected ILogger Logger{ get; private set; }

        private IEventBus eventBus;
        protected IEventBus EventBus
        {
            get
            {
                if (eventBus == null)
                {
                    eventBus = ServiceLocator.GetInstance<IEventBus>();
                }
                return eventBus;
            }
        }

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

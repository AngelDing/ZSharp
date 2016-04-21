using System;
using ZSharp.Framework.Logging;

namespace ZSharp.Framework.RabbitMq
{
    public abstract class BaseRabbitMq : DisposableObject
    {
        protected ILogger Logger{ get; private set; }        
        
        protected IEventBus eventBus { get; set; }

        protected IClientCommandDispatcher clientCommandDispatcher { get; set; }

        protected readonly ConnectionConfiguration connectionConfiguration;

        public BaseRabbitMq()
        {
            Logger = LogManager.GetLogger(GetType());
            connectionConfiguration = GetConnectionConfiguration();
        }

        private ConnectionConfiguration GetConnectionConfiguration()
        {
            throw new NotImplementedException();
        }
    }
}

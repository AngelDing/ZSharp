using ZSharp.Framework.Configurations;

namespace ZSharp.Framework.RabbitMq
{
    public interface IClientCommandDispatcherFactory
    {
        IClientCommandDispatcher GetClientCommandDispatcher(IPersistentConnection connection);
    }

    public class ClientCommandDispatcherFactory : IClientCommandDispatcherFactory
    {
        private readonly IRabbitMqConfiguration configuration;
        private readonly IPersistentChannelFactory persistentChannelFactory;

        public ClientCommandDispatcherFactory(
            IRabbitMqConfiguration configuration,
            IPersistentChannelFactory persistentChannelFactory)
        {
            this.configuration = configuration;
            this.persistentChannelFactory = persistentChannelFactory;
        }

        public IClientCommandDispatcher GetClientCommandDispatcher(IPersistentConnection connection)
        {
            return new ClientCommandDispatcher(configuration, connection, persistentChannelFactory);
        }
    }
}
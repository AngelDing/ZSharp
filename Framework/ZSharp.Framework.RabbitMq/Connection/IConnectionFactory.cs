using RabbitMQ.Client;
using ZSharp.Framework.Configurations;

namespace ZSharp.Framework.RabbitMq
{
    public interface IConnectionFactory
    {
        IConnection CreateConnection();

        IRabbitMqConfiguration Configuration { get; }

        RabbitMqHost CurrentHost { get; }

        bool Next();

        void Success();

        void Reset();

        bool Succeeded { get; }
    }

    public class ConnectionFactoryWrapper : IConnectionFactory
    {
        public virtual IRabbitMqConfiguration Configuration { get; private set; }
        private readonly IClusterHostSelectionStrategy<ConnectionFactoryInfo> clusterHostSelectionStrategy;

        public ConnectionFactoryWrapper(
            IRabbitMqConfiguration connectionConfiguration, 
            IClusterHostSelectionStrategy<ConnectionFactoryInfo> clusterHostSelectionStrategy)
        {
            this.clusterHostSelectionStrategy = clusterHostSelectionStrategy;
            Configuration = connectionConfiguration;

            foreach (RabbitMqHost hostConfiguration in Configuration.RabbitMqHosts)
            {
                var connectionFactory = new ConnectionFactory
                {
                    UseBackgroundThreadsForIO = connectionConfiguration.UseBackgroundThreads,
                    AutomaticRecoveryEnabled = false,
                    TopologyRecoveryEnabled = false
                };

                connectionFactory.HostName = hostConfiguration.Ip;
                
                if(connectionFactory.VirtualHost == "/")
                    connectionFactory.VirtualHost = Configuration.VirtualHost;
                
                if(connectionFactory.UserName == "guest")
                    connectionFactory.UserName = Configuration.UserName;

                if(connectionFactory.Password == "guest")
                    connectionFactory.Password = Configuration.Password;

                if (connectionFactory.Port == -1)
                    connectionFactory.Port = hostConfiguration.Port;

                connectionFactory.RequestedHeartbeat = Configuration.RequestedHeartbeat;
                connectionFactory.ClientProperties = Configuration.ClientProperties;
                clusterHostSelectionStrategy.Add(new ConnectionFactoryInfo(connectionFactory, hostConfiguration));
            }
        }

        public virtual IConnection CreateConnection()
        {
            return clusterHostSelectionStrategy.Current().ConnectionFactory.CreateConnection();
        }

        public virtual RabbitMqHost CurrentHost
        {
            get { return clusterHostSelectionStrategy.Current().HostConfiguration; }
        }

        public virtual bool Next()
        {
            return clusterHostSelectionStrategy.Next();
        }

        public virtual void Reset()
        {
            clusterHostSelectionStrategy.Reset();
        }

        public virtual void Success()
        {
            clusterHostSelectionStrategy.Success();
        }

        public virtual bool Succeeded
        {
            get { return clusterHostSelectionStrategy.Succeeded; }
        }
    }

    public class ConnectionFactoryInfo
    {
        public ConnectionFactoryInfo(ConnectionFactory connectionFactory, RabbitMqHost hostConfiguration)
        {
            ConnectionFactory = connectionFactory;
            HostConfiguration = hostConfiguration;
        }

        public ConnectionFactory ConnectionFactory { get; private set; }
        public RabbitMqHost HostConfiguration { get; private set; }
    }

}
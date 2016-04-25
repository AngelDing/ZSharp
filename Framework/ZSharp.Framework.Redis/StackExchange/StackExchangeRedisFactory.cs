using StackExchange.Redis;
using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Configuration;
using ZSharp.Framework.Configurations;

namespace ZSharp.Framework.Redis
{
    public class StackExchangeRedisFactory
    {
        private static ConcurrentDictionary<string, ConnectionMultiplexer> connectionDic = new ConcurrentDictionary<string, ConnectionMultiplexer>();
        private IRedisConfiguration redisConfiguration;
        private string redisConfigName;
        private static readonly object SyncLock = new object();

        static StackExchangeRedisFactory()
        {
            AppDomain.CurrentDomain.DomainUnload += DomainUnload;
        }

        static void DomainUnload(object sender, EventArgs e)
        {
            if (connectionDic != null)
            {
                connectionDic.Keys.ToList().ForEach(key => {
                    ConnectionMultiplexer c = null;
                    if (connectionDic.TryRemove(key, out c))
                    {
                        c.Dispose();
                        c = null;
                    }
                });
                connectionDic = null;
            }
        }

        public StackExchangeRedisFactory(string redisConfigName)
        {
            var configuration = RedisConfigurationHandler.GetConfig(redisConfigName);            

            if (configuration == null)
            {
                var msg = "Unable to locate <redisConfig> section into your configuration file.";
                throw new ConfigurationErrorsException(msg);
            }
            this.redisConfigName = redisConfigName;
            this.redisConfiguration = configuration;
        }

        public IDatabase GetDatabase()
        {
            var connection = ConstructCacheInstance();
            return connection.GetDatabase(redisConfiguration.Database);
        }

        private ConnectionMultiplexer ConstructCacheInstance()
        {
            var connection = connectionDic.GetOrAdd(
                redisConfigName, (config) => GetConnetionMultiplexer(config));

            if (IsConnected(connection) == false)
            {
                connection = connectionDic.AddOrUpdate(
                    redisConfigName, 
                    (key) => { return null; }, 
                    (key, oldConnection) =>
                    {
                        if (oldConnection != null)
                        {
                            if (IsConnected(oldConnection) == true)
                            {
                                return oldConnection;
                            }
                            else
                            {
                                oldConnection.Dispose();
                            }
                        }
                        return GetConnetionMultiplexer(key);
                    });
            }

            return connection;
        }

        private bool IsConnected(ConnectionMultiplexer connection)
        {
            var isConnected = true;
            if (connection == null || connection.IsConnected == false
                || connection.GetDatabase().IsConnected(default(RedisKey)) == false)
            {
                isConnected = false;
            }

            return isConnected;
        }

        private ConnectionMultiplexer GetConnetionMultiplexer(string config)
        {
            var connectionOptions = ConstructConnectionOptions();
            lock (SyncLock)
            {                
                try
                {
                    var connection = ConnectionMultiplexer.Connect(connectionOptions);
                    return connection;
                }
                catch (Exception ex)
                {
                    //Logger.WriteException(ex);
                    throw ex;
                }
            }
        }

        private ConfigurationOptions ConstructConnectionOptions()
        {
            var redisOptions = new ConfigurationOptions
            {
                Ssl = redisConfiguration.Ssl,
                AllowAdmin = redisConfiguration.AllowAdmin,
                ConnectTimeout = redisConfiguration.ConnectTimeout,
                KeepAlive = 5,
                //DefaultVersion = new Version("2.8.19"),
                Proxy = Proxy.None
            };

            foreach (RedisHost redisHost in redisConfiguration.RedisHosts)
            {
                redisOptions.EndPoints.Add(redisHost.Ip, redisHost.Port);
            }
            return redisOptions;
        }
    }
}

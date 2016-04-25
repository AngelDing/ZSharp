using System;
using System.Collections.Generic;
using System.Configuration;

namespace ZSharp.Framework.Configurations
{
    public class RabbitMqHostGroup : ConfigurationElement, IRabbitMqConfiguration
    {       
        [ConfigurationProperty("virtualHost", IsRequired = true, IsKey = true, DefaultValue = "/")]
        public string VirtualHost
        {
            get
            {
                return this["virtualHost"] as string;
            }
        }

        [ConfigurationProperty("userName", IsRequired = true, DefaultValue = "guest")]
        public string UserName
        {
            get
            {
                return this["userName"] as string;
            }
        }

        [ConfigurationProperty("password", IsRequired = true, DefaultValue = "guest")]
        public string Password
        {
            get
            {
                return this["password"] as string;
            }
        }

        [ConfigurationProperty("publisherConfirms")]
        public bool PublisherConfirms
        {
            get
            {
                bool result = false;
                var config = this["publisherConfirms"];
                if (config != null)
                {
                    var value = config.ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        if (bool.TryParse(value, out result))
                        {
                            return result;
                        }
                    }
                }
                return result;
            }
        }

        [ConfigurationProperty("persistentMessages")]
        public bool PersistentMessages
        {
            get
            {
                bool result = true;
                var config = this["persistentMessages"];
                if (config != null)
                {
                    var value = config.ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        if (bool.TryParse(value, out result))
                        {
                            return result;
                        }
                    }
                }
                return result;
            }
        }

        [ConfigurationProperty("cancelOnHaFailover")]
        public bool CancelOnHaFailover
        {
            get
            {
                bool result = false;
                var config = this["cancelOnHaFailover"];
                if (config != null)
                {
                    var value = config.ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        if (bool.TryParse(value, out result))
                        {
                            return result;
                        }
                    }
                }
                return result;
            }
        }

        [ConfigurationProperty("mandatory")]
        public bool Mandatory
        {
            get
            {
                bool result = false;
                var config = this["mandatory"];
                if (config != null)
                {
                    var value = config.ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        if (bool.TryParse(value, out result))
                        {
                            return result;
                        }
                    }
                }
                return result;
            }
        }

        [ConfigurationProperty("useBackgroundThreads")]
        public bool UseBackgroundThreads
        {
            get
            {
                bool result = false;
                var config = this["useBackgroundThreads"];
                if (config != null)
                {
                    var value = config.ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        if (bool.TryParse(value, out result))
                        {
                            return result;
                        }
                    }
                }
                return result;
            }
        }

        [ConfigurationProperty("prefetchCount")]
        public ushort PrefetchCount
        {
            get
            {
                var config = this["prefetchCount"];
                if (config != null)
                {
                    var value = config.ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        ushort result;
                        if (ushort.TryParse(value, out result))
                        {
                            return result;
                        }
                    }
                }
                return 50;
            }
        }

        [ConfigurationProperty("requestedHeartbeat")]
        public ushort RequestedHeartbeat
        {
            get
            {
                var config = this["requestedHeartbeat"];
                if (config != null)
                {
                    var value = config.ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        ushort result;
                        if (ushort.TryParse(value, out result))
                        {
                            return result;
                        }
                    }
                }
                return 10;
            }
        }

        [ConfigurationProperty("timeout")]
        public ushort Timeout
        {
            get
            {
                var config = this["timeout"];
                if (config != null)
                {
                    var value = config.ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        ushort result;
                        if (ushort.TryParse(value, out result))
                        {
                            return result;
                        }
                    }
                }
                return 10;
            }
        }

        [ConfigurationProperty("hosts")]
        public RabbitMqHostCollection RabbitMqHosts
        {
            get
            {
                return this["hosts"] as RabbitMqHostCollection;
            }
        }

        [ConfigurationProperty("clientProperties")]
        public RabbitMqClientPropertyCollection ClientPropertyCollection
        {
            get
            {
                return this["clientProperties"] as RabbitMqClientPropertyCollection;
            }
        }

        public IDictionary<string, object> ClientProperties
        {
            get
            {
                var clientProperties = new Dictionary<string, object>();
                foreach (RabbitMqClientProperty p in ClientPropertyCollection)
                {
                    clientProperties.Add(p.Key, p.Value);
                }
                return clientProperties;
            }
        }
    }
}
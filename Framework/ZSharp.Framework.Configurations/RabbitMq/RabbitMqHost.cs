using System.Configuration;
using ZSharp.Framework.Extensions;

namespace ZSharp.Framework.Configurations
{
    public class RabbitMqHost : ConfigurationElement
    {
        private const int DefaultPort = 5672;

        [ConfigurationProperty("ip", IsRequired = true)]
        public string Ip
        {
            get
            {
                return this["ip"] as string;
            }
        }

        [ConfigurationProperty("port", IsRequired = true)]
        public int Port
        {
            get
            {
                var port = DefaultPort;
                var config = this["port"];
                if (config != null)
                {
                    var value = config.ToString();

                    if (!value.IsNullOrEmpty())
                    {
                        port = value.ToInt(5672);                      
                    }
                }
                return port;
            }
        }

        public string HostFullName
        {
            get
            {
                return string.Format("{0}:{1}", Ip, Port);
            }
        }
    }
}
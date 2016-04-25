using System.Configuration;
using ZSharp.Framework.Extensions;

namespace ZSharp.Framework.Configurations
{
    public class RabbitMqHost : ConfigurationElement
    {
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
                var port = 5672;
                var config = this["port"];
                if (config != null)
                {
                    var value = config.ToString();

                    if (!value.IsNullOrEmpty())
                    {
                        port = value.ToInt(port);                      
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
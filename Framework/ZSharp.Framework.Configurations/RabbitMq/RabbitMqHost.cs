using System.Configuration;
using ZSharp.Framework.Extensions;

namespace ZSharp.Framework.Configurations
{
    public class RabbitMqHost : ConfigurationElement
    {
        /// <summary>
        /// 可以是Ip，也可以是域名
        /// </summary>
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
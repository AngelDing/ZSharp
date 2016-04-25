using System;
using System.Configuration;

namespace ZSharp.Framework.Configurations
{
	public class RedisHost : ConfigurationElement
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
                var config = this["port"];
				if (config != null)
				{
					var value = config.ToString();

					if (!string.IsNullOrEmpty(value))
					{
						int result;

						if (int.TryParse(value, out result))
						{
							return result;
						}
					}
				}

				throw new Exception("Redis Cahe port must be number.");
			}
		}

        public string HostFullName
        {
            get
            {
                return string.Format("{0}:{1}", this.Ip, this.Port);
            }
        }
	}
}
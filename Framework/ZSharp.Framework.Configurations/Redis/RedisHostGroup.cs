using System.Configuration;

namespace ZSharp.Framework.Configurations
{
    public class RedisHostGroup : ConfigurationElement, IRedisConfiguration
	{       
		[ConfigurationProperty("hosts")]      
		public RedisHostCollection RedisHosts
		{ 
			get
			{
				return this["hosts"] as RedisHostCollection;
			}
		}

        [ConfigurationProperty("name", IsRequired = true, IsKey = true, DefaultValue = "System")]
        public string Name
        {
            get
            {
                return this["name"] as string;
            }
        }

		[ConfigurationProperty("allowAdmin")]
		public bool AllowAdmin
		{
			get
			{
				bool result = false;
				var config = this["allowAdmin"];

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

		[ConfigurationProperty("ssl")]
		public bool Ssl
		{
			get
			{
				bool result = false;
				var config = this["ssl"];
				if (config != null)
				{
					var value = config.ToString();
					if (!string.IsNullOrWhiteSpace(value))
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

		[ConfigurationProperty("connectTimeout")]
		public int ConnectTimeout
		{
			get
			{
				var config = this["connectTimeout"];
				if (config != null)
				{
					var value = config.ToString();
					if (!string.IsNullOrWhiteSpace(value))
					{
						int result;
						if (int.TryParse(value, out result))
						{
							return result;
						}
					}
				}

				return 5000;
			}
		}

		[ConfigurationProperty("database")]
		public int Database
		{
			get
			{
				var config = this["database"];
				if (config != null)
				{
					var value = config.ToString();
					if (!string.IsNullOrWhiteSpace(value))
					{
						int result;
						if (int.TryParse(value, out result))
						{
							return result;
						}
					}
				}

				return 0;
			}
		}
	}
}
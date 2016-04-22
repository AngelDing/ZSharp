using System;
using System.Configuration;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.Configurations
{
    public class RabbitMqConfigurationHandler : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public RedisHostGroupCollection HostGroups
        {
            get
            {
                return this[""] as RedisHostGroupCollection;
            }
        }

        public static IRedisConfiguration GetConfig(string redisConfigName)
        {
            GuardHelper.ArgumentNotNull(() => redisConfigName);

            var handler = ConfigurationManager.GetSection("redisConfig") as RedisConfigurationHandler;
            GuardHelper.ArgumentNotNull(() => handler);
            RedisHostGroup group = null;

            foreach (RedisHostGroup item in handler.HostGroups)
            {
                if (item.Name.Equals(redisConfigName, StringComparison.OrdinalIgnoreCase))
                {
                    group = item;
                    break;
                }
            }

            if (group == null)
            {
                throw new Exception(string.Format("Redis配置错误，根據服務器配置組名:{0}找不到Redis服務器组", redisConfigName));
            }

            return group;
        }
    }
}

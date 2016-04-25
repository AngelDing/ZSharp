using System;
using System.Configuration;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.Configurations
{
    public class RabbitMqConfigurationHandler : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public RabbitMqHostGroupCollection HostGroups
        {
            get
            {
                return this[""] as RabbitMqHostGroupCollection;
            }
        }

        public static IRabbitMqConfiguration GetConfig(string rabbitMqConfigName)
        {
            GuardHelper.ArgumentNotNull(() => rabbitMqConfigName);
            var handler = ConfigurationManager.GetSection("rabbitMqConfig") as RabbitMqConfigurationHandler;
            GuardHelper.ArgumentNotNull(() => handler);
            RabbitMqHostGroup group = null;

            foreach (RabbitMqHostGroup item in handler.HostGroups)
            {
                if (item.VirtualHost.Equals(rabbitMqConfigName, StringComparison.OrdinalIgnoreCase))
                {
                    group = item;
                    break;
                }
            }
            if (group == null)
            {
                throw new Exception(string.Format("RabbitMq配置错误，根據配置名:{0}找不到RabbitMq服務器组", rabbitMqConfigName));
            }

            return group;
        }
    }
}

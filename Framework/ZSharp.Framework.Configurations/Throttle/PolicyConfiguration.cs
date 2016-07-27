using System.Configuration;

namespace ZSharp.Framework.Configurations.Throttle
{
    public class PolicyConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("limitPerSecond", DefaultValue = "0", IsRequired = false)]
        [LongValidator(ExcludeRange = false, MinValue = 0)]
        public long LimitPerSecond
        {
            get
            {
                return (long)this["limitPerSecond"];
            }
        }
        [ConfigurationProperty("endpointThrottlingType", DefaultValue = "ControllerAndAction", IsRequired = false)]
        public string EndpointThrottlingType
        {
            get
            {
                return this["endpointThrottlingType"] as string;
            }
        }
        [ConfigurationProperty("limitPerMinute", DefaultValue = "0", IsRequired = false)]
        [LongValidator(ExcludeRange = false, MinValue = 0)]
        public long LimitPerMinute
        {
            get
            {
                return (long)this["limitPerMinute"];
            }
        }

        [ConfigurationProperty("limitPerHour", DefaultValue = "0", IsRequired = false)]
        [LongValidator(ExcludeRange = false, MinValue = 0)]
        public long LimitPerHour
        {
            get
            {
                return (long)this["limitPerHour"];
            }
        }

        [ConfigurationProperty("limitPerDay", DefaultValue = "0", IsRequired = false)]
        [LongValidator(ExcludeRange = false, MinValue = 0)]
        public long LimitPerDay
        {
            get
            {
                return (long)this["limitPerDay"];
            }
        }

        [ConfigurationProperty("limitPerWeek", DefaultValue = "0", IsRequired = false)]
        [LongValidator(ExcludeRange = false, MinValue = 0)]
        public long LimitPerWeek
        {
            get
            {
                return (long)this["limitPerWeek"];
            }
        }

        [ConfigurationProperty("ipThrottling", DefaultValue = "false", IsRequired = false)]
        public bool IpThrottling
        {
            get
            {
                return (bool)this["ipThrottling"];
            }
        }

        [ConfigurationProperty("clientThrottling", DefaultValue = "false", IsRequired = false)]
        public bool ClientThrottling
        {
            get
            {
                return (bool)this["clientThrottling"];
            }
        }

        [ConfigurationProperty("endpointThrottling", DefaultValue = "false", IsRequired = false)]
        public bool EndpointThrottling
        {
            get
            {
                return (bool)this["endpointThrottling"];
            }
        }

        [ConfigurationProperty("stackBlockedRequests", DefaultValue = "false", IsRequired = false)]
        public bool StackBlockedRequests
        {
            get
            {
                return (bool)this["stackBlockedRequests"];
            }
        }

        [ConfigurationProperty("rules")]
        public RuleConfigurationCollection Rules
        {
            get
            {
                return this["rules"] as RuleConfigurationCollection;
            }
        }

        [ConfigurationProperty("whitelists")]
        public WhitelistConfigurationCollection Whitelists
        {
            get
            {
                return this["whitelists"] as WhitelistConfigurationCollection;
            }
        }
    }
}

using System.Configuration;

namespace ZSharp.Framework.Configurations.Throttle
{
    public class WhitelistConfigurationCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new WhitelistConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((WhitelistConfigurationElement)element).Entry;
        }
    }
}

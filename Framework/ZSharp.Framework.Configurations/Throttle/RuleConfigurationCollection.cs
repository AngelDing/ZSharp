using System.Configuration;

namespace ZSharp.Framework.Configurations.Throttle
{
    public class RuleConfigurationCollection : ConfigurationElementCollection
    {

        protected override ConfigurationElement CreateNewElement()
        {
            return new RuleConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RuleConfigurationElement)element).Entry;
        }
    }
}

using System.Configuration;

namespace ZSharp.Framework.Configurations
{
    public class RabbitMqHostGroupCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new RabbitMqHostGroup();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RabbitMqHostGroup)element).VirtualHost;
        }

        protected override string ElementName
        {
            get { return "hostGroup"; }
        }
    }
}

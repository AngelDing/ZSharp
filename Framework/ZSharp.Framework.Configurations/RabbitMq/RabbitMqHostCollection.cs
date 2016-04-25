using System.Configuration;

namespace ZSharp.Framework.Configurations
{
    public class RabbitMqHostCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new RabbitMqHost();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var config = ((RabbitMqHost)element);
            return config.HostFullName;
        }

        protected override string ElementName
        {
            get { return "host"; }
        }
    }
}

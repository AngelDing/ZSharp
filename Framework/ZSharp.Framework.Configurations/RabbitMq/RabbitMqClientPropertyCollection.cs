using System.Configuration;

namespace ZSharp.Framework.Configurations
{
    public class RabbitMqClientPropertyCollection : ConfigurationElementCollection
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
            return new RabbitMqClientProperty();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var config = ((RabbitMqClientProperty)element);
            return config.Key;
        }

        protected override string ElementName
        {
            get { return "clientProperty"; }
        }
    }
}
using System.Configuration;

namespace ZSharp.Framework.Configurations
{
    public class RedisHostGroupCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new RedisHostGroup();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RedisHostGroup)element).Name;
        }

        protected override string ElementName
        {
            get { return "hostGroup"; }
        }
    }
}

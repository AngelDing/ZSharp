using System.Configuration;

namespace FlyweightPattern.ObjectPool
{
    [ConfigurationCollection(typeof(PoolableConfigurationElement),
    CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    class PoolableConfigurationElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Public const
        /// </summary>
        public const string Name = "settings";


        /// <summary>
        /// Indexer by index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public PoolableConfigurationElement this[int index]
        {
            get { return (PoolableConfigurationElement)base.BaseGet(index); }
        }

        /// <summary>
        /// Indexer by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public new PoolableConfigurationElement this[string name]
        {
            get { return base.BaseGet(name) as PoolableConfigurationElement; }
        }

        /// <summary>
        /// Create new config elemnet instance.
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new PoolableConfigurationElement();
        }

        /// <summary>
        /// Get config element key
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as PoolableConfigurationElement).TypeName;
        }

    }
}

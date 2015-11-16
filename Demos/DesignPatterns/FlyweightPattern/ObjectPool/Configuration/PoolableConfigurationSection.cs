using System.Configuration;

namespace FlyweightPattern.ObjectPool
{
    /// <summary>
    /// Root configuration section of ObjectPool
    /// </summary>
    class PoolableConfigurationSection : ConfigurationSection
    {
        public const string Name = "objectPool";

        /// <summary>
        /// Public property 
        /// </summary>
        [ConfigurationProperty(PoolableConfigurationElementCollection.Name)]
        public PoolableConfigurationElementCollection Settings
        {
            get
            {
                return base[PoolableConfigurationElementCollection.Name]
                    as PoolableConfigurationElementCollection;
            }
        }
    }
}

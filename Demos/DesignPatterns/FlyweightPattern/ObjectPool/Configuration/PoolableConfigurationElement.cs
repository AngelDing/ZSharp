using System;
using System.Configuration;

namespace FlyweightPattern.ObjectPool
{
    /// <summary>
    /// Each poolable item pooling setting
    /// </summary>
    class PoolableConfigurationElement : ConfigurationElement
    {
        #region Private Field
        private const string TypeItem = "type";
        private const string MaxItem = "max";
        private const string TimeoutItem = "timeout";
        #endregion


        /// <summary>
        /// Poolable type
        /// </summary>
        [ConfigurationProperty(TypeItem, IsRequired = true, IsKey=true)]
        public string TypeName
        {
            get { return base[TypeItem] as string; }
        }

        /// <summary>
        /// Max poolable instances
        /// </summary>
        [ConfigurationProperty(MaxItem, IsRequired = true)]
        public int Max
        {
            get { return Convert.ToInt32(base[MaxItem]); }
        }

        /// <summary>
        /// Timeout from the lastest invoke
        /// </summary>
        [ConfigurationProperty(TimeoutItem, IsRequired=true)]
        public int Timeout
        {
            get { return Convert.ToInt32(base[TimeoutItem]); }
        }
    }
}

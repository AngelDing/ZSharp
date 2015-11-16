using System;
using System.Collections.Generic;

namespace FlyweightPattern.ObjectPool
{
    /// <summary>
    /// 对象池配置管理器
    /// </summary>
    public sealed class ConfigManager
    {
        /// <summary>
        /// Singleton 的实例
        /// </summary>
        private static ConfigManager instance;

        /// <summary>
        /// 缓冲的池化类型配置信息
        /// </summary>
        private IDictionary<Type, IPoolableConfiguration> cache;


        /// <summary>
        /// 静态构造
        /// </summary>
        static ConfigManager()
        {
            instance = new ConfigManager();
            PoolableConfigurationSection section = 
                System.Configuration.ConfigurationManager.GetSection(PoolableConfigurationSection.Name) 
                as PoolableConfigurationSection;
            instance.cache = new Dictionary<Type, IPoolableConfiguration>();
            foreach (PoolableConfigurationElement element in section.Settings)
            {
                Type type = Type.GetType(element.TypeName);

                // check whether is qualified IPoolable interface
                if (typeof(IPoolable).IsAssignableFrom(type))   
                    instance.cache.Add(type, new PoolableConfigurationItem(element.Max, element.Timeout));
                else
                    throw new TypeInitializationException(typeof(IPoolable).FullName, null);
            }
        }


        /// <summary>
        /// Singleton 实例引用
        /// </summary>
        public static ConfigManager Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// 获取指定池化类型的配置信息
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IPoolableConfiguration this[Type type]
        {
            get
            {
                if ((cache == null) || (cache.Count <= 0)) return null;
                IPoolableConfiguration item;
                if (!cache.TryGetValue(type, out item))
                    return null;
                else
                    return item;
            }
        }
    }
}

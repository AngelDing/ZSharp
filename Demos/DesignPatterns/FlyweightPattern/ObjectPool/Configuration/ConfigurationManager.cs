using System;
using System.Collections.Generic;

namespace FlyweightPattern.ObjectPool
{
    /// <summary>
    /// ��������ù�����
    /// </summary>
    public sealed class ConfigManager
    {
        /// <summary>
        /// Singleton ��ʵ��
        /// </summary>
        private static ConfigManager instance;

        /// <summary>
        /// ����ĳػ�����������Ϣ
        /// </summary>
        private IDictionary<Type, IPoolableConfiguration> cache;


        /// <summary>
        /// ��̬����
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
        /// Singleton ʵ������
        /// </summary>
        public static ConfigManager Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// ��ȡָ���ػ����͵�������Ϣ
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

using System;
using System.Collections.Generic;

namespace FlyweightPattern.ObjectPool
{
    /// <summary>
    /// 池化对象缓冲
    /// </summary>
    public class ObjectCache
    {
        /// <summary>
        /// 相关类型缓冲列表注册器
        /// </summary>
        private static IDictionary<Type, object> registry;

        /// <summary>
        /// 静态构造类型列表注册表
        /// </summary>
        static ObjectCache()
        {
            registry = new Dictionary<Type, object>();
        }

        /// <summary>
        /// 获取一个可用的缓冲对象
        /// </summary>
        /// <typeparam name="T">指定的对象类型</typeparam>
        /// <param name="item">可用的对象实例</param>
        /// <param name="increasable">是否可以继续添加</param>
        /// <returns>是否成功获得可用的实例</returns>
        public bool TryToAcquire<T>(out T item, out bool increasable) where T : PoolableBase, new()
        {
            PrepareTypeList<T>();
            return (registry[typeof(T)] as SizeRestrictedList<T>).Acquire(out item, out increasable);
        }

        /// <summary>
        /// 缓冲新的实例
        /// </summary>
        /// <param name="item"></param>
        public void Cache<T>(T item) where T : PoolableBase, new()
        {
            PrepareTypeList<T>();
            (registry[typeof(T)] as SizeRestrictedList<T>).Add(item);
        }

        /// <summary>
        /// 准备特定类型缓冲列表
        /// </summary>
        /// <param name="type"></param>
        private void PrepareTypeList<T>() where T : PoolableBase, new()
        {
            if (!registry.ContainsKey(typeof(T)))
            {
                registry.Add(typeof(T), new SizeRestrictedList<T>());
            }
        }
    }
}

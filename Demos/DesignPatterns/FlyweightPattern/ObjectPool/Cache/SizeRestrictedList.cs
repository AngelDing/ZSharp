using System;
using System.Collections.Generic;
using System.Threading;

namespace FlyweightPattern.ObjectPool
{
    /// <summary>
    /// 缓冲数量限制的池化对象缓冲 List
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SizeRestrictedList<T> where T : PoolableBase, new()
    {
        /// <summary>
        /// 相关缓冲内容
        /// </summary>
        private IPoolableConfiguration configuration;

        /// <summary>
        /// 自动清理时钟对象
        /// </summary>
        private Timer timer;

        /// <summary>
        /// 对象实例缓冲 List
        /// </summary>
        private IList<T> cache;

        /// <summary>
        /// 根据配置情况构造特定类型的缓冲 List
        /// </summary>
        /// <param name="typeName">实际缓冲的对象类型名称</param>
        public SizeRestrictedList()
        {
            configuration = ConfigManager.Instance[typeof(T)];
            cache = new List<T>();
            TimerCallback callback = new TimerCallback(ClearUp);
            timer = new Timer(callback, null, 0, configuration.Timeout);
        }

        /// <summary>
        /// 获取一个新的实例
        /// </summary>
        /// <param name="instance">可用的对象实例</param>
        /// <param name="increasable">是否可以继续添加</param>
        /// <returns>是否成功获得可用的实例</returns>
        public bool Acquire(out T item, out bool increasable)
        {
            increasable = cache.Count >= configuration.Max ? false : true;
            item = null;

            // 无法获取缓冲对象
            if (cache.Count <= 0)
                return false;


            // 重用既有实例
            foreach (T cachedItem in cache)
            {
                if ((cachedItem != null) && (cachedItem.Unoccupied))
                {
                    item = cachedItem;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 增加新的缓冲实例
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            if ((cache.Count < configuration.Max) || (item != null))  // 已经达到数量上限
            {
                cache.Add(item);
            }
        }

        /// <summary>
        /// 清理超期未使用对象
        /// </summary>
        private void ClearUp(object sender)
        {
            if (cache.Count <= 0) return;

            // 查找
            List<int> toDeleteList = new List<int>();
            for(int i=0; i<cache.Count; i++)
            {
                TimeSpan timeSpan = DateTime.Now.Subtract(cache[i].AccessedTime);
                if (timeSpan.TotalMilliseconds > configuration.Timeout)
                {
                    toDeleteList.Add(i);
                }
            }

            // 清理
            if (toDeleteList.Count <= 0)
            {
                return;
            }
            for (int i = toDeleteList.Count - 1; i >= 0; i--)
            {
                T item = cache[toDeleteList[i]];
                cache.Remove(item);
                item.Dispose();
                item = null;
            }
        }
    }
}

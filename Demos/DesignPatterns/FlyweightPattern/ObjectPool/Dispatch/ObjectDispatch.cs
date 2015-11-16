
namespace FlyweightPattern.ObjectPool
{
    /// <summary>
    /// 对象池调度器
    /// </summary>
    public static class ObjectDispatch
    {
        /// <summary>
        /// 是否可以对外服务
        /// </summary>
        private static bool available = true;

        /// <summary>
        /// 根据每个类型实例的使用情况，获取指定类型的对象实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Acquire<T>() where T : PoolableBase, new()
        {
            available = false;
            T item = null;
            bool increasable;

            ObjectCache cache = new ObjectCache();
            if (!(cache.TryToAcquire<T>(out item, out increasable)))
            {
                if (increasable)
                {
                    ObjectBuilder<T> builder = new ObjectBuilder<T>();
                    item = builder.BuildUp();           
                    cache.Cache<T>(item);
                }
            }

            available = true;
            return item;
        }


        /// <summary>
        /// 是否可以对外服务
        /// </summary>
        public static bool Available
        {
            get { return available; } 
        }
    }
}


namespace FlyweightPattern.ObjectPool
{
    /// <summary>
    /// ����ص�����
    /// </summary>
    public static class ObjectDispatch
    {
        /// <summary>
        /// �Ƿ���Զ������
        /// </summary>
        private static bool available = true;

        /// <summary>
        /// ����ÿ������ʵ����ʹ���������ȡָ�����͵Ķ���ʵ��
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
        /// �Ƿ���Զ������
        /// </summary>
        public static bool Available
        {
            get { return available; } 
        }
    }
}

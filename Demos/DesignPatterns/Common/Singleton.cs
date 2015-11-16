using System;

namespace Common
{
    /// <summary>
    /// 標示對象是單實例的
    /// </summary>
    public interface ISingleton
    {
    }

    public abstract class SingletonBase<T> : ISingleton
        where T : ISingleton, new()
    {
        //[ThreadStatic]  // 说明每个Instance仅在当前线程内静态,如果加此标签，则不需要锁；
        private static T instance;
        private static readonly object syncRoot = new object();

        protected SingletonBase()
        {
        }

        public static T Instance
        {
           get
            {
                // double-check locking
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            // use lazy initialization
                            instance = new T();
                        }
                    }
                }
                return instance;
            }
        }
    }

    public class SingletonSimple : SingletonBase<SingletonSimple>
    {
        public void Test()
        {
            Console.WriteLine("Singleton Test");
        }
    }

    public class SingletonClient
    {
        public void Test()
        {
            SingletonSimple.Instance.Test();
        }
    }
}

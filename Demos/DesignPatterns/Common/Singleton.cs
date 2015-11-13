namespace Common
{
    /// <summary>
    /// 標示對象是單實例的
    /// </summary>
    public interface ISingleton
    {
    }

    public class Singleton : ISingleton
    {
        private static Singleton instance;
        private static readonly object syncRoot = new object();

        // the constructor should be protected or private
        private Singleton()
        {
        }

        public static Singleton Instance()
        {
            // double-check locking
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        // use lazy initialization
                        instance = new Singleton();
                    }
                }
            }
            return instance;
        }
    }
}

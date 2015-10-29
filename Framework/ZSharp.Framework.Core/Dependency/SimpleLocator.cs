
namespace ZSharp.Framework.Dependency
{
    public class SimpleLocator<T> where T : SimpleLocator, new()
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                }
                return instance;
            }
        }

        public static IContainer Current
        {
            get { return Instance.Container; }
        }        
    }

    public class SimpleLocator
    {
        private IContainer container;

        public SimpleLocator()
        {
            container = new SimpleContainer();
            RegisterDefaults(container);
        }

        public IContainer Container
        {
            get { return container; }
        }
        
        public virtual void RegisterDefaults(IContainer container)
        {
        }        
    }
}

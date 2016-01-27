using Microsoft.Practices.Unity;
using Locator = Microsoft.Practices.ServiceLocation;

namespace ZSharp.Framework.Infrastructure
{
    public static class ServiceLocator
    {
        public static void SetLocatorProvider(IUnityContainer container)
        {
            //http://blogs.msdn.com/b/miah/archive/2009/05/12/servicelocator-and-unity-be-careful.aspx
            //http://unity.codeplex.com/workitem/11791
            UnityServiceLocator locator = new UnityServiceLocator(container);
            Locator.ServiceLocator.SetLocatorProvider(() => locator);
        }

        public static TService GetInstance<TService>()
        {
            return Locator.ServiceLocator.Current.GetInstance<TService>();
        }

        public static TService GetInstance<TService>(string key)
        {
            return Locator.ServiceLocator.Current.GetInstance<TService>(key);
        }
    }
}

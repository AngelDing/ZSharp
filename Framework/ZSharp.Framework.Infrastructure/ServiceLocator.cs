using Microsoft.Practices.Unity;
using Locator = Microsoft.Practices.ServiceLocation;

namespace ZSharp.Framework.Infrastructure
{
    public static class ServiceLocator
    {
        public static void SetLocatorProvider(IUnityContainer container)
        {
            Locator.ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));
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

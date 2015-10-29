using ZSharp.Framework.Dependency;

namespace ZSharp.Framework.Caching
{
    public static class CacheHelper
    {
        public static ICacheManager WebCache
        {
            get
            {
                return SimpleLocator<CacheLocator>.Current.Resolve<CacheManager<AspNetCache>>();
            }
        }

        public static ICacheManager MemoryCache
        {
            get
            {
                return SimpleLocator<CacheLocator>.Current.Resolve<CacheManager<StaticCache>>();
            }
        }

        public static ICacheManager RedisCache
        {
            get 
            {
                return SimpleLocator<CacheLocator>.Current.Resolve<CacheManager<RedisCache>>();
            }
        }

        public static ICacheManager LRUCache
        {
            get
            {
                return SimpleLocator<CacheLocator>.Current.Resolve<CacheManager<LRUCache>>();
            }
        }
    }
}

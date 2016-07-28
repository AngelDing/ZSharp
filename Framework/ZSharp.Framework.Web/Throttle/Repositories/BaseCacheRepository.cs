using ZSharp.Framework.Caching;

namespace ZSharp.Framework.Web.Throttle
{
    public abstract class BaseCacheRepository
    {
        protected ICacheManager CacheManager { get; private set; }

        public BaseCacheRepository()
        {
            CacheManager = GetCacheManager();
        }

        protected abstract ICacheManager GetCacheManager();
    }
}

using ZSharp.Framework.Caching;

namespace ZSharp.Framework.Web.Throttle
{
    /// <summary>
    /// Stors throttle metrics in runtime cache, intented for owin self host.
    /// </summary>
    public class MemoryCacheThrottleRepository : CacheThrottleRepository
    {
        protected override ICacheManager GetCacheManager()
        {
            return CacheHelper.MemoryCache;
        }
    }
}

using ZSharp.Framework.Caching;

namespace ZSharp.Framework.Web.Throttle
{
    /// <summary>
    /// Stors throttle metrics in runtime cache, intented for owin self host.
    /// </summary>
    public class ThrottleMemoryCacheRepository : ThrottleBaseRepository
    {
        protected override ICacheManager CacheManager()
        {
            return CacheHelper.MemoryCache;
        }
    }
}

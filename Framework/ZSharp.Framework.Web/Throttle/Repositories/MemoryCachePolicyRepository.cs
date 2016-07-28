using ZSharp.Framework.Caching;

namespace ZSharp.Framework.Web.Throttle
{
    /// <summary>
    /// Stores policy in runtime cache, intended for OWIN self host.
    /// </summary>
    public class MemoryCachePolicyRepository : CachePolicyRepository
    {
        protected override ICacheManager GetCacheManager()
        {
            return CacheHelper.MemoryCache;
        }
    }
}

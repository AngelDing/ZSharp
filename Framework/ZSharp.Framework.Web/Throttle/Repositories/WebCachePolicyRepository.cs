using ZSharp.Framework.Caching;

namespace ZSharp.Framework.Web.Throttle
{
    /// <summary>
    /// Stores policy in asp.net cache
    /// </summary>
    public class WebCachePolicyRepository : CachePolicyRepository
    {
        protected override ICacheManager GetCacheManager()
        {
            return CacheHelper.WebCache;
        }
    }
}

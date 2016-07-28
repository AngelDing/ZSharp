using ZSharp.Framework.Caching;

namespace ZSharp.Framework.Web.Throttle
{
    /// <summary>
    /// Stores throttle metrics in asp.net cache
    /// </summary>
    public class WebCacheThrottleRepository : CacheThrottleRepository
    {
        protected override ICacheManager GetCacheManager()
        {
            return CacheHelper.WebCache;
        }
    }
}

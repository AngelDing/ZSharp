using ZSharp.Framework.Caching;

namespace ZSharp.Framework.Web.Throttle
{
    /// <summary>
    /// Stores throttle metrics in asp.net cache
    /// </summary>
    public class ThrottleWebCacheRepository : ThrottleBaseRepository
    {
        protected override ICacheManager CacheManager()
        {
            return CacheHelper.WebCache;
        }
    }
}

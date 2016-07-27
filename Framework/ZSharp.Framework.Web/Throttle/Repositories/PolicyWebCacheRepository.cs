using ZSharp.Framework.Caching;

namespace ZSharp.Framework.Web.Throttle
{
    /// <summary>
    /// Stores policy in asp.net cache
    /// </summary>
    public class PolicyWebCacheRepository : PolicyBaseRepository
    {
        protected override ICacheManager CacheManager()
        {
            return CacheHelper.WebCache;
        }
    }
}

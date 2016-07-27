using ZSharp.Framework.Caching;

namespace ZSharp.Framework.Web.Throttle
{
    /// <summary>
    /// Stores policy in runtime cache, intended for OWIN self host.
    /// </summary>
    public class PolicyMemoryCacheRepository : PolicyBaseRepository
    {
        protected override ICacheManager CacheManager()
        {
            return CacheHelper.MemoryCache;
        }
    }
}

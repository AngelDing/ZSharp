using System;
using ZSharp.Framework.Caching;

namespace ZSharp.Framework.Web.Throttle
{
    public abstract class CacheThrottleRepository : BaseCacheRepository, IThrottleRepository
    {
        /// <summary>
        /// Insert or update
        /// </summary>
        public void Save(string id, ThrottleCounter throttleCounter, TimeSpan expirationTime)
        {
            var cachePolicy = CachePolicy.WithSlidingExpiration(expirationTime);
            CacheManager.Set(id, throttleCounter, cachePolicy);
        }

        public bool Any(string id)
        {
            return CacheManager.Contains(id);
        }

        public ThrottleCounter? FirstOrDefault(string id)
        {
            return CacheManager.Get<ThrottleCounter>(id);
        }

        public void Remove(string id)
        {
            CacheManager.Remove(id);
        }

        public void Clear()
        {
            CacheManager.ClearAll();
        }
    }
}

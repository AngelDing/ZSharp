using System;
using ZSharp.Framework.Caching;

namespace ZSharp.Framework.Web.Throttle
{
    public abstract class ThrottleBaseRepository : IThrottleRepository
    {
        private readonly ICacheManager cacheManager;

        public ThrottleBaseRepository()
        {
            cacheManager = CacheManager();
        }

        protected abstract ICacheManager CacheManager();

        /// <summary>
        /// Insert or update
        /// </summary>
        public void Save(string id, ThrottleCounter throttleCounter, TimeSpan expirationTime)
        {
            var cachePolicy = CachePolicy.WithSlidingExpiration(expirationTime);
            cacheManager.Set(id, throttleCounter, cachePolicy);
        }

        public bool Any(string id)
        {
            return cacheManager.Contains(id);
        }

        public ThrottleCounter? FirstOrDefault(string id)
        {
            return cacheManager.Get<ThrottleCounter>(id);
        }

        public void Remove(string id)
        {
            cacheManager.Remove(id);
        }

        public void Clear()
        {
            cacheManager.ClearAll();
        }
    }
}

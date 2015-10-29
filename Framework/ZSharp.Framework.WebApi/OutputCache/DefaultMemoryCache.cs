using ZSharp.Framework.Extensions;
using ZSharp.Framework.Caching;
using System;
using System.Collections.Generic;

namespace ZSharp.Framework.WebApi.OutputCache
{
    public class DefaultMemoryCache : IApiOutputCache
    {
        private ICacheManager cacheManager;
        public DefaultMemoryCache()
        {
            cacheManager = CacheHelper.MemoryCache;
        }

        public void RemoveStartsWith(string key)
        {
            cacheManager.RemoveByPattern(key);
        }

        public T Get<T>(string key) where T : class
        {
            return cacheManager.Get<T>(key);
        }

        public void Remove(string key)
        {
            cacheManager.Remove(key);
        }

        public bool Contains(string key)
        {
            return cacheManager.Contains(key);
        }

        public void Add(string key, object o, DateTimeOffset expiration, string dependsOnKey = null)
        {
            var cacheKey = new CacheKey(key);
            if (!dependsOnKey.IsNullOrEmpty())
            {
                cacheKey = new CacheKey(key, new List<string> { dependsOnKey });
            }

            var cachePolicy = CachePolicy.WithAbsoluteExpiration(expiration);
            cacheManager.Set(cacheKey, o, cachePolicy);
        }
    }
}

using System;
using System.Threading.Tasks;

namespace ZSharp.Framework.Caching
{
    public interface ICacheManager
    {
        T Get<T>(string key, Func<T> acquirer = null, CachePolicy cachePolicy = null, bool IsAllowDirtyRead = false);

        Task<T> GetAsync<T>(string key, Func<T> acquirer = null, CachePolicy cachePolicy = null, bool IsAllowDirtyRead = false);

        T Get<T>(CacheKey key, Func<T> acquirer = null, CachePolicy cachePolicy = null, bool IsAllowDirtyRead = false);

        Task<T> GetAsync<T>(CacheKey key, Func<T> acquirer = null, CachePolicy cachePolicy = null, bool IsAllowDirtyRead = false);

        void Set(string key, object value, CachePolicy cachePolicy = null);

        Task SetAsync(string key, object value, CachePolicy cachePolicy = null);

        void Set(CacheKey key, object value, CachePolicy cachePolicy = null);

        Task SetAsync(CacheKey key, object value, CachePolicy cachePolicy = null);

        void Remove(string key);

        Task RemoveAsync(string key);

        void RemoveByPattern(string pattern);

        Task RemoveByPatternAsync(string pattern);

        void Expire(string tag);

        Task ExpireAsync(string tag);

        void ClearAll();

        Task ClearAllAsync();

        CacheType CacheType { get; }

        bool Contains(string key);
    }
}

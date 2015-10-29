using System;
using System.Threading.Tasks;

namespace ZSharp.Framework.Caching
{
    public abstract class BaseCacheManager : ICacheManager
    {
        #region Abstract Methods        
        public abstract T DoGet<T>(string key, Func<T> acquirer, CachePolicy cachePolicy, bool IsAllowDirtyRead);
        public abstract T DoGet<T>(CacheKey key, Func<T> acquirer, CachePolicy cachePolicy, bool IsAllowDirtyRead);
        public abstract void DoSet(string key, object value, CachePolicy cachePolicy);
        public abstract void DoSet(CacheKey key, object value, CachePolicy cachePolicy);
        public abstract void DoRemove(string key);
        public abstract void DoRemoveByPattern(string pattern);
        public abstract void DoExpire(string tag);
        public abstract void DoClearAll();
        public abstract CacheType GetCacheType();
        public abstract bool DoContains(string key);
        #endregion

        public T Get<T>(string key, Func<T> acquirer = null, CachePolicy cachePolicy = null, bool IsAllowDirtyRead = false)
        {
            return  DoGet<T>(key, acquirer, cachePolicy, IsAllowDirtyRead);
        }

        public Task<T> GetAsync<T>(string key, Func<T> acquirer = null, CachePolicy cachePolicy = null, bool IsAllowDirtyRead = false) 
        {
            return Task.FromResult(this.Get(key, acquirer, cachePolicy, IsAllowDirtyRead));
        }

        public T Get<T>(CacheKey key, Func<T> acquirer = null, CachePolicy cachePolicy = null, bool IsAllowDirtyRead = false) 
        {
            return DoGet<T>(key, acquirer, cachePolicy, IsAllowDirtyRead);
        }

        public Task<T> GetAsync<T>(CacheKey key, Func<T> acquirer = null, CachePolicy cachePolicy = null, bool IsAllowDirtyRead = false) 
        {
            return Task.FromResult(this.Get(key, acquirer, cachePolicy, IsAllowDirtyRead));
        }

        public void Set(string key, object value, CachePolicy cachePolicy = null)
        {
            DoSet(key, value, cachePolicy);
        }

        public Task SetAsync(string key, object value, CachePolicy cachePolicy = null)
        {
            return Task.Run(() => this.Set(key, value, cachePolicy));
        }

        public void Set(CacheKey key, object value, CachePolicy cachePolicy = null)
        {
            DoSet(key, value, cachePolicy);
        }

        public Task SetAsync(CacheKey key, object value, CachePolicy cachePolicy = null)
        {
            return Task.Run(() => this.Set(key, value, cachePolicy));
        }

        public void Remove(string key)
        {
            DoRemove(key);
        }

        public Task RemoveAsync(string key)
        {
            return Task.Run(() => this.Remove(key));
        }

        public void RemoveByPattern(string pattern)
        {
            DoRemoveByPattern(pattern);
        }

        public Task RemoveByPatternAsync(string pattern)
        {
            return Task.Run(() => RemoveByPattern(pattern));
        }

        public void Expire(string tag)
        {
            DoExpire(tag);
        }

        public Task ExpireAsync(string tag)
        {
            return Task.Run(() => Expire(tag));
        }

        public void ClearAll()
        {
            DoClearAll();
        }

        public Task ClearAllAsync()
        {
            return Task.Run(() => ClearAll());
        }

        public CacheType CacheType
        {
            get
            {
                return GetCacheType();
            }
        }        

        public bool Contains(string key)
        {
            return DoContains(key);
        }
    }
}

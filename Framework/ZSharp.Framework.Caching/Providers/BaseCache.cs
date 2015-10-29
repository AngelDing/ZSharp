using System.Linq;
using System.Collections.Generic;
using System;
using System.Threading;
using ZSharp.Framework.Threading;
using ZSharp.Framework.Extensions;

namespace ZSharp.Framework.Caching
{
    public abstract class BaseCache
    {
        private readonly ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();

        internal IList<string> GetTagKeyList(CacheKey key)
        {
           return  key.Tags.Select(GetTagKey).ToList();            
        }

        internal string GetTagKey(CacheTag t)
        {
            return string.Format(CacheKeyTemplates.CacheTag, t);
        } 

        internal string GetKey(CacheKey cacheKey)
        {
            return cacheKey.Key;
        }

        public abstract bool IsSingleton();

        public abstract T Get<T>(string key);

        public abstract void Set(CacheKey cacheKey, object value, CachePolicy cachePolicy);

        public abstract void Remove(string key);

        public IDisposable EnterReadLock()
        {
            return IsSingleton() ? rwLock.GetUpgradeableReadLock() : ActionDisposable.Empty;
        }

        public IDisposable EnterWriteLock()
        {
            return IsSingleton() ? rwLock.GetWriteLock() : ActionDisposable.Empty;
        }

        public virtual void Expire(CacheTag cacheTag)
        {
            string key = GetTagKey(cacheTag);
            var childKeys = this.Get<List<string>>(key);
            if (childKeys.IsNullOrEmpty() == false)
            {
                childKeys.ForEach(k => Remove(k));
            }
            Remove(key);
        }
        public void ManageCacheDependencies(CacheKey cacheKey)
        {
            if (cacheKey.Tags.Any())
            {
                foreach (var tag in cacheKey.Tags)
                {
                    var tagStr = GetTagKey(tag);
                    var childKeys = this.Get<List<string>>(tagStr);
                    if (childKeys.IsNullOrEmpty() == true)
                    {
                        childKeys = new List<string>();
                    }
                    var childKey = cacheKey.Key;
                    if (childKeys.Contains(childKey) == false)
                    {
                        childKeys.Add(childKey);
                    }
                    var tagKey = new CacheKey(tagStr);
                    var cachePolicy = CachePolicy.WithAbsoluteExpiration(DateTimeOffset.UtcNow.AddYears(10));
                    this.Set(tagKey, childKeys, cachePolicy);
                }
            }
        }

    }
}

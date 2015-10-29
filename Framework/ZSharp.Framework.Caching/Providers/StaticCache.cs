using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using ZSharp.Framework.Extensions;
using ZSharp.Framework.Configurations;

namespace ZSharp.Framework.Caching
{    
    public partial class StaticCache : CommonCache, ICacheProvider
    {       
		private ObjectCache _cache;

        protected ObjectCache Cache
        {
            get
            {
				if (_cache == null)
				{
                    var codeType = CommonConfig.SystemCode;
                    if (codeType.IsNullOrEmpty())
                    {
                        codeType = "DefaultMemoryCache";
                    }
                    _cache = new MemoryCache(codeType);
				}
				return _cache;
            }
        }

        public override IEnumerable<KeyValuePair<string, object>> GetAllEntries()
        {
            return Cache;
        }

        public override bool IsSingleton()
        { 
            return false;
        }

        public override T Get<T>(string key)
        {
			var data = Cache.Get(key);
            return (T)data;
        }

        public override void Set(CacheKey cacheKey, object value, CachePolicy cachePolicy)
        {
            string strKey = GetKey(cacheKey);
            var cacheItem = new CacheItem(strKey, value);
            var policy = CreatePolicy(cacheKey, cachePolicy);

            Cache.Set(cacheItem, policy);
        }

        public bool Contains(string key)
        {
            return Cache.Contains(key);
        }

        public override void Remove(string key)
        {
            Cache.Remove(key);
        }

        public override void Expire(CacheTag cacheTag)
        {
            string key = GetTagKey(cacheTag);
            var item = new CacheItem(key, DateTimeOffset.UtcNow.Ticks);
            var policy = new CacheItemPolicy { AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration };

            Cache.Set(item, policy);

            this.Remove(key);
        }		 

        internal CacheItemPolicy CreatePolicy(CacheKey key, CachePolicy cachePolicy)
        {
            var policy = new CacheItemPolicy();

            switch (cachePolicy.ExpirationType)
            {
                case CacheExpirationType.Sliding:
                    policy.SlidingExpiration = cachePolicy.SlidingExpiration;
                    break;
                case CacheExpirationType.Absolute:
                    policy.AbsoluteExpiration = cachePolicy.AbsoluteExpiration;
                    break;
                case CacheExpirationType.Duration:
                    policy.AbsoluteExpiration = DateTimeOffset.UtcNow.Add(cachePolicy.Duration);
                    break;
                default:
                    policy.AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration;
                    break;
            }

            var changeMonitor = CreateChangeMonitor(key);
            if (changeMonitor != null)
            {
                policy.ChangeMonitors.Add(changeMonitor);
            }

            return policy;
        }

        internal CacheEntryChangeMonitor CreateChangeMonitor(CacheKey key)
        {
            var tags = GetTagKeyList(key);

            if (tags.Count == 0)
            {
                return null;
            }

            var absoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration;
            var value = DateTimeOffset.UtcNow.Ticks;
            foreach (string tag in tags)
            {
                Cache.Add(tag, value, absoluteExpiration);
            }

            return Cache.CreateCacheEntryChangeMonitor(tags);
        }

        public CacheType CacheType
        {
            get { return CacheType.Memory; }
        }  
    }
}

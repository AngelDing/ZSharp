using System;
using System.Linq;
using System.Collections.Generic;
using ZSharp.Framework.Serializations;
using ZSharp.Framework.Extensions;
using ZSharp.Framework.Redis;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.Caching
{
    public class RedisCache : BaseCache, ICacheProvider
    {
        private readonly ISerializer serializer;
        private readonly IRedisWrapper redisWrapper;

        public RedisCache()
            : this(SerializationHelper.MsgPack)
        { 
        }

        public RedisCache(ISerializer serializer, IRedisWrapper redisWrapper = null)
        {
            GuardHelper.ArgumentNotNull(() => serializer);
            if (redisWrapper == null)
            {
                redisWrapper = RedisFactory.GetRedisWrapper();
            }

            this.serializer = serializer;
            this.redisWrapper = redisWrapper;
        }

        public CacheType CacheType
        {
            get { return CacheType.Redis; }
        }

        public override T Get<T>(string key) 
        {
            var data = redisWrapper.Get(key);
            if (data != null)
            {
                var deserialisedObject = serializer.Deserialize<T>(data);
                return deserialisedObject;
            }

            return default(T);
        }

        public override void Set(CacheKey key, object value, CachePolicy cachePolicy)
        {
            var dataStr = serializer.Serialize<string>(value);
            var expiry = ComputeExpiryTimeSpan(cachePolicy);
            redisWrapper.Set(key.Key, dataStr, expiry);
            ManageCacheDependencies(key);
        }

        private TimeSpan? ComputeExpiryTimeSpan(CachePolicy cachePolicy)
        {
            TimeSpan? expiry = null;
            switch (cachePolicy.ExpirationType)
            {
                case CacheExpirationType.Sliding:
                    expiry = cachePolicy.SlidingExpiration;
                    break;
                case CacheExpirationType.Absolute:
                    expiry = cachePolicy.AbsoluteExpiration - DateTimeOffset.UtcNow;
                    break;
                case CacheExpirationType.Duration:
                    expiry = cachePolicy.Duration;
                    break;
                default:
                    break;
            }

            return expiry;
        }

        public bool Contains(string key)
        {
            return redisWrapper.Exists(key);
        }

        public override void Remove(string key)
        {
            redisWrapper.Remove(key);
        }

        /// <summary>
        /// 批量刪除緩存
        /// </summary>
        /// <param name="pattern">模式匹配</param>
        /// <example>
        /// if you want to return all keys that start with "myCacheKey" uses "myCacheKey*"
        ///	if you want to return all keys that contain with "myCacheKey" uses "*myCacheKey*"
        ///	if you want to return all keys that end with "myCacheKey" uses "*myCacheKey"
        /// </example>
        public void RemoveByPattern(string pattern)
        {
            redisWrapper.RemoveByPattern(pattern);
        }

        public void ClearAll()
        {
            redisWrapper.ClearAll();
        }

        public override bool IsThreadSafety()
        {
            return true;
        }       
    }
}
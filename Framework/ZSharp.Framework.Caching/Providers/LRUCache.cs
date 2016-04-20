using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System;
using System.Threading.Tasks;
using ZSharp.Framework.Configurations;

namespace ZSharp.Framework.Caching
{
    /// <summary>
    /// 關於LRUCache的過期策略問題：由於是定時清理，故存在一定的時間誤差，不是嚴格意義上的過期時間一到就失效，
    /// 具體參考測試用例；相關內容也請參考 : http://www.cnblogs.com/mushroom/p/4278275.html
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LRUCache : CommonCache, ICacheProvider
    {
        private long ageToDiscard = 0;        //淘汰的年龄起点
        private long currentAge = 0;          //当前缓存最新年龄
        private int maxSize = 0;              //缓存最大容量
        private TimeSpan maxExpireTime;       //最大过期时间
        private static ConcurrentDictionary<string, TrackValue> cache;
        private static Task heartBit;

        public LRUCache()
        {
            maxExpireTime = TimeSpan.FromHours(24);
            maxSize = CommonConfig.LRUCacheMaxSize;
            cache = new ConcurrentDictionary<string, TrackValue>();

            heartBit = new Task(Inspection, TaskCreationOptions.LongRunning);
            heartBit.Start();
        }

        public override T Get<T>(string key)
        {
            T result = default(T);

            if (this.Contains(key) == false)
            {
                return result;
            }

            var tupleValue = CheckExpire(key);           

            if (tupleValue.Item2 == true)
            {
                result = (T)tupleValue.Item1.Value;
            }

            return result;
        }

        public override void Set(CacheKey key, object value, CachePolicy cachePolicy)
        {
            var strKey = key.Key;
            Adjust(strKey);
            var expireTime = ComputeExpireTime(cachePolicy);
            var result = new TrackValue(this, value, expireTime);
            cache.AddOrUpdate(strKey, result, (k, o) => result);

            ManageCacheDependencies(key);
        }

        public bool Contains(string key)
        {
            return cache.ContainsKey(key);
        }

        #region Override Methods

        public override IEnumerable<KeyValuePair<string, object>> GetAllEntries()
        {
            var result =
                from entry in cache
                let key = entry.Key
                select new KeyValuePair<string, object>(key, entry.Value);

            return result;
        }

        public override void Remove(string key)
        {
            TrackValue old;
            cache.TryRemove(key, out old);
        }

        public override bool IsThreadSafety()
        {
            return true;
        }

        #endregion 

        #region Private Methods

        private void Inspection()
        {
            while (true)
            {
                var sleepSeconds = CommonConfig.LRUCacheCleaningIntervalSeconds;
                Thread.Sleep(TimeSpan.FromSeconds(sleepSeconds));
                foreach (var item in cache)
                {
                    CheckExpire(item.Key);
                }
            }
        }       

        private class TrackValue
        {
            public DateTimeOffset CreateTime { get; private set; }
            public TimeSpan ExpireTime { get; private set; }
            public readonly object Value;
            public long Age;

            public TrackValue(LRUCache lv, object tv, TimeSpan? exprieTime = null)
            {
                Age = Interlocked.Increment(ref lv.currentAge);
                Value = tv;
                if (exprieTime.HasValue)
                {
                    ExpireTime = exprieTime.Value;
                }
                else
                {
                    ExpireTime = TimeSpan.FromMinutes(5);
                }

                CreateTime = DateTimeOffset.Now;
            }
        }

        public CacheType CacheType
        {
            get { return CacheType.ConcurrentDictionary; }
        }

        private TimeSpan? ComputeExpireTime(CachePolicy cachePolicy)
        {
            TimeSpan? timeSpan = null;
            switch (cachePolicy.ExpirationType)
            {
                case CacheExpirationType.Absolute:
                    timeSpan = cachePolicy.AbsoluteExpiration - DateTimeOffset.UtcNow;
                    break;
                case CacheExpirationType.Duration:
                    timeSpan = cachePolicy.Duration;
                    break;
                case CacheExpirationType.Sliding:
                    timeSpan = cachePolicy.SlidingExpiration;
                    break;
            }
            return timeSpan;
        }

        /// <summary>
        /// 檢查緩存項吃否已到過期時間，如果到了過期時間則移除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private Tuple<TrackValue, bool> CheckExpire(string key)
        {           
            TrackValue result = default(TrackValue);
            var expireVlue = Tuple.Create(result, false);
            var tupleValue = expireVlue;

            if (cache.TryGetValue(key, out result))
            {
                var age = DateTimeOffset.Now.Subtract(result.CreateTime);
                if (age >= maxExpireTime || age >= result.ExpireTime)
                {
                    TrackValue old;
                    cache.TryRemove(key, out old);
                }
                else
                {
                    tupleValue = Tuple.Create(result, true);
                }
            }

            return tupleValue;
        }

        /// <summary>
        /// 超過存儲上限時，將很長時間未被訪問的value，移除出緩存
        /// </summary>
        /// <param name="key"></param>
        private void Adjust(string key)
        {
            while (cache.Count >= maxSize)
            {
                long ageToDelete = Interlocked.Increment(ref ageToDiscard);
                var toDiscard = cache.FirstOrDefault(p => p.Value.Age == ageToDelete);
                if (toDiscard.Key == null)
                {
                    continue;
                }
                TrackValue old;
                cache.TryRemove(toDiscard.Key, out old);
            }
        }

        #endregion    
    }   
}

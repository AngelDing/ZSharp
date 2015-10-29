using System;
using System.Threading;
using System.Threading.Tasks;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.Caching
{
    public class CacheManager<TCache> : BaseCacheManager where TCache : ICacheProvider
    {
		private readonly ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();
		private readonly ICacheProvider cacheProvider;
        private const int cacheSignValue = 1;

        public CacheManager(Func<Type, ICacheProvider> fn)
        {
            this.cacheProvider = fn(typeof(TCache));
        } 

        public override T DoGet<T>(string key, Func<T> acquirer, CachePolicy cachePolicy, bool IsAllowDirtyRead)
        {
            GuardHelper.ArgumentNotEmpty(() => key);
            var cacheKey = new CacheKey(key);
            return DoGet(cacheKey, acquirer, cachePolicy, IsAllowDirtyRead);
        }

        public override T DoGet<T>(CacheKey key, Func<T> acquirer, CachePolicy cachePolicy, bool IsAllowDirtyRead)
        {
            var strKey = key.Key;
            GuardHelper.ArgumentNotEmpty(() => strKey);

            if (cachePolicy == null)
            {
                cachePolicy = new CachePolicy();
            }

            if (IsAllowDirtyRead)
            {
                return GetCacheValueByDirtyRead<T>(key, acquirer, cachePolicy);
            }
            else
            {
                return GetCahcheValueByCommonRead<T>(key, acquirer, cachePolicy);
            }
        }

        #region Private Methods
        /// <summary>
        /// 正常讀取緩存數據：緩存中沒有，則執行獲取數據方法
        /// </summary>
        /// <typeparam name="T">緩存值類型</typeparam>
        /// <param name="cacheKey">key</param>
        /// <param name="acquirer">獲取原始數據方法</param>
        /// <param name="cachePolicy">緩存過期策略</param>
        /// <returns>緩存結果</returns>
        private T GetCahcheValueByCommonRead<T>(CacheKey cacheKey, Func<T> acquirer, CachePolicy cachePolicy)
        {
            string strKey = cacheKey.Key;
            if (cacheProvider.Contains(strKey))
            {
                var value = cacheProvider.Get<T>(strKey);
                ProlongCacheValueLifeTime(cacheKey, cachePolicy, value);
                return value;
            }
            else
            {
                if (acquirer == null)
                {
                    return default(T);
                }
                using (cacheProvider.EnterReadLock())
                {
                    if (!cacheProvider.Contains(strKey))
                    {
                        var value = acquirer();
                        this.Set(cacheKey, value, cachePolicy);

                        return value;
                    }
                }

                return this.Get<T>(strKey);
            }
        }       

        /// <summary>
        /// 允許賍讀緩存結果：額外提供一個Key：cacheSign，緩存時間小於真實的緩存對象；
        /// 先判斷cacheSign是否過期，過期則異步更新真實數據，同時返回尚未更新的緩存對象（賍數據）；
        /// </summary>
        /// <typeparam name="T">緩存值類型</typeparam>
        /// <param name="cacheKey">key</param>
        /// <param name="acquirer">獲取原始數據方法</param>
        /// <param name="cachePolicy">緩存過期策略</param>
        /// <returns>緩存結果</returns>
        private T GetCacheValueByDirtyRead<T>(CacheKey cacheKey, Func<T> acquirer, CachePolicy cachePolicy)
        {
            string strKey = cacheKey.Key;
            string cacheSign = string.Intern(string.Format(CacheKeyTemplates.CacheSign, strKey));
            T cacheValue = default(T);
            var isExistDirtyData = cacheProvider.Contains(strKey);
            if (isExistDirtyData == true)
            {
                cacheValue = GetCacheValue<T>(strKey, cachePolicy);
            }

            if (IsCacheSignExpired(cacheSign, cachePolicy) == false)
            {
                return cacheValue;
            }

            T newCacheValue = default(T);
            lock (cacheSign)
            {
                if (IsCacheSignExpired(cacheSign, cachePolicy) == false)
                {
                    return cacheValue;
                }

                this.Set(cacheSign, cacheSignValue, cachePolicy);

                newCacheValue = RealSetCacheValue(cacheKey, acquirer, cachePolicy, isExistDirtyData);
            }

            if (isExistDirtyData == false)
            {
                cacheValue = newCacheValue;
            }
            return cacheValue;
        }

        /// <summary>
        /// 更新緩存數據,如果存在賍數據，則執行異步執行更新；
        /// 否則同步執行更新，同時返回獲取的真實數據（這種情況出現在第一次訪問，或者sign key和realkey均過期時)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="acquirer"></param>
        /// <param name="cachePolicy"></param>
        /// <param name="isExistDirtyData"></param>
        /// <returns></returns>
        private T RealSetCacheValue<T>(CacheKey cacheKey, Func<T> acquirer, CachePolicy cachePolicy, bool isExistDirtyData)
        {
            T result = default(T);
            if (acquirer == null)
            {
                return result;
            }
            if (isExistDirtyData == false)
            {
                result = GetAndSetRealData<T>(cacheKey, acquirer, cachePolicy);
            }
            else
            {
                ThreadPool.QueueUserWorkItem((arg) =>
                {
                    GetAndSetRealData<T>(cacheKey, acquirer, cachePolicy);
                });
            }
            return result;
        }

        private T GetAndSetRealData<T>(CacheKey cacheKey, Func<T> acquirer, CachePolicy cachePolicy)
        {
            T result = acquirer();
            var newCachePolicy = CreateNewCachePolicy(cachePolicy);
            this.Set(cacheKey, result, newCachePolicy);
            return result;
        }

        /// <summary>
        /// 新的過期時間要比CacheSign的過期時間要長,用於賍讀,默認延長一天
        /// </summary>
        /// <param name="cachePolicy"></param>
        /// <returns></returns>
        private CachePolicy CreateNewCachePolicy(CachePolicy cachePolicy)
        {
            var newPlicy = new CachePolicy();

            switch(cachePolicy.ExpirationType)
            {
                case CacheExpirationType.Absolute:
                    newPlicy.AbsoluteExpiration = cachePolicy.AbsoluteExpiration.AddDays(1);
                    break;
                case CacheExpirationType.Duration:
                    newPlicy.Duration = cachePolicy.Duration.Add(new TimeSpan(24, 0, 0));
                    break;
                case CacheExpirationType.Sliding:
                    newPlicy.SlidingExpiration = cachePolicy.SlidingExpiration.Add(new TimeSpan(24,0,0));
                    break;
            }

            return newPlicy;
        }

        /// <summary>
        /// 獲取緩存數據
        /// </summary>
        private T GetCacheValue<T>(string key, CachePolicy cachePolicy)
        {
            T value = default(T);
            var isContains = false;

            isContains = this.Contains(key);
            if (isContains == true)
            {
                value = this.Get<T>(key);
                ProlongCacheValueLifeTime(new CacheKey(key), cachePolicy, value);
            }

            return value;
        }

        /// <summary>
        /// 如果是相對過期時間，且是Redis/LUR時，在獲取數據的同時更新其到期時間
        /// </summary>
        private void ProlongCacheValueLifeTime(CacheKey cacheKey, CachePolicy cachePolicy, object value)
        {
            if (cachePolicy.ExpirationType == CacheExpirationType.Sliding
                && (this.CacheType == CacheType.Redis || this.CacheType == CacheType.ConcurrentDictionary))
            {
                this.Set(cacheKey, value, cachePolicy);
            }
        }

        private bool IsCacheSignExpired(string cacheSign, CachePolicy cachePolicy)
        {
            var isExpired = false;
            var value = GetCacheValue<int>(cacheSign, cachePolicy);

            if (value != cacheSignValue)
            {
                isExpired = true;
            }

            return isExpired;
        }

        #endregion

        public override void DoSet(string key, object value, CachePolicy cachePolicy)
        {
            GuardHelper.ArgumentNotEmpty(() => key);
            var cacheKey = new CacheKey(key);
            DoSet(cacheKey, value, cachePolicy);
        }

        public override void DoSet(CacheKey cacheKey, object value, CachePolicy cachePolicy)
        {
            var strKey = cacheKey.Key;
            GuardHelper.ArgumentNotEmpty(() => strKey);

            if (value == null)
            {
                return;
            }

            if (cachePolicy == null)
            {
                cachePolicy = new CachePolicy();
            }

            using (cacheProvider.EnterWriteLock())
            {
                cacheProvider.Set(cacheKey, value, cachePolicy);
            }
        }

        public override void DoRemove(string key)
        {
            GuardHelper.ArgumentNotEmpty(() => key);

            RealRemove(key);

            //如果採用臟讀寫入數據時，刪除時應該把相應的CacheSign的數據同時刪除
            var signKey = string.Format(CacheKeyTemplates.CacheSign, key);
            if (this.Contains(signKey))
            {
                RealRemove(signKey);
            }
        }

        private void RealRemove(string key)
        {
            using (cacheProvider.EnterWriteLock())
            {
                cacheProvider.Remove(key);
            }
        }

        public override void DoRemoveByPattern(string pattern)
        {
            cacheProvider.RemoveByPattern(pattern);
        }

        public override void DoClearAll()
        {
            cacheProvider.ClearAll();
        }

        public override void DoExpire(string tag)
        {
            var cacheTag = new CacheTag(tag);
            cacheProvider.Expire(cacheTag);
        }

        public override CacheType GetCacheType()
        {
            return cacheProvider.CacheType; 
        }

        public override bool DoContains(string key)
        {
            return cacheProvider.Contains(key);
        }
    }
}
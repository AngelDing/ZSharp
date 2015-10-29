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
        /// �����xȡ���攵���������Л]�У��t���Ы@ȡ��������
        /// </summary>
        /// <typeparam name="T">����ֵ���</typeparam>
        /// <param name="cacheKey">key</param>
        /// <param name="acquirer">�@ȡԭʼ��������</param>
        /// <param name="cachePolicy">�����^�ڲ���</param>
        /// <returns>����Y��</returns>
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
        /// ���S�_�x����Y�����~���ṩһ��Key��cacheSign������r�gС��挍�ľ��挦��
        /// ���Д�cacheSign�Ƿ��^�ڣ��^�ڄt���������挍������ͬ�r������δ���µľ��挦���_��������
        /// </summary>
        /// <typeparam name="T">����ֵ���</typeparam>
        /// <param name="cacheKey">key</param>
        /// <param name="acquirer">�@ȡԭʼ��������</param>
        /// <param name="cachePolicy">�����^�ڲ���</param>
        /// <returns>����Y��</returns>
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
        /// ���¾��攵��,��������_�������t���Ю������и��£�
        /// ��tͬ�����и��£�ͬ�r���ث@ȡ���挍�������@�N��r���F�ڵ�һ���L��������sign key��realkey���^�ڕr)
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
        /// �µ��^�ڕr�gҪ��CacheSign���^�ڕr�gҪ�L,����_�x,Ĭ�J���Lһ��
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
        /// �@ȡ���攵��
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
        /// ����������^�ڕr�g������Redis/LUR�r���ګ@ȡ������ͬ�r�����䵽�ڕr�g
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

            //��������K�x���딵���r���h���r��ԓ��������CacheSign�Ĕ���ͬ�r�h��
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
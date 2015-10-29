using System;
using System.Threading.Tasks;

namespace ZSharp.Framework.Caching
{
    public partial class NullCacheManager : BaseCacheManager
    {
		private readonly static ICacheManager s_instance = new NullCacheManager();

		public static ICacheManager Instance
		{
			get { return s_instance; } 
		}

        public override T DoGet<T>(string key, Func<T> acquirer, CachePolicy cachePolicy, bool IsAllowDirtyRead)
        {
            return this.Get(new CacheKey(key), acquirer, cachePolicy);
        }

        public override T DoGet<T>(CacheKey key, Func<T> acquirer, CachePolicy cachePolicy, bool IsAllowDirtyRead)
        {            
            if (acquirer == null)
            {
                return default(T);
            }
            return acquirer();
        }

        public override void DoRemove(string key)
        {
        }

        public override void DoRemoveByPattern(string pattern)
        {
        }

        public override void DoClearAll()
        {
        }

        public override void DoSet(string key, object value, CachePolicy cachePolicy = null)
        {
        }

        public override void DoSet(CacheKey key, object value, CachePolicy cachePolicy = null)
        {
        }

        public override void DoExpire(string tag)
        {
        }

        public override CacheType GetCacheType()
        {
            return CacheType.None;
        }

        public override bool DoContains(string key)
        {
            return false;
        }      
    }
}
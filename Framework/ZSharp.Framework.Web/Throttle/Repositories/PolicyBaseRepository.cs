using ZSharp.Framework.Caching;

namespace ZSharp.Framework.Web.Throttle
{
    public abstract class PolicyBaseRepository : IPolicyRepository
    {
        private readonly ICacheManager cacheManager;

        public PolicyBaseRepository()
        {
            cacheManager = CacheManager();
        }

        protected abstract ICacheManager CacheManager();

        public void Save(string id, ThrottlePolicy policy)
        {
            cacheManager.Set(id, policy);
        }

        public ThrottlePolicy FirstOrDefault(string id)
        {
            return cacheManager.Get<ThrottlePolicy>(id);
        }

        public void Remove(string id)
        {
            cacheManager.Remove(id);
        }
    }
}

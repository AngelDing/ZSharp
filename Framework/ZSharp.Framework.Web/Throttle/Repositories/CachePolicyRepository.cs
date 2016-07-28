
namespace ZSharp.Framework.Web.Throttle
{
    public abstract class CachePolicyRepository : BaseCacheRepository, IPolicyRepository
    {
        public void Save(string id, ThrottlePolicy policy)
        {
            CacheManager.Set(id, policy);
        }

        public ThrottlePolicy FirstOrDefault(string id)
        {
            return CacheManager.Get<ThrottlePolicy>(id);
        }

        public void Remove(string id)
        {
            CacheManager.Remove(id);
        }
    }
}

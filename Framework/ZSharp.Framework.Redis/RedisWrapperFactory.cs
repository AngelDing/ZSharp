using ZSharp.Framework.Configurations;
using ZSharp.Framework.Extensions;

namespace ZSharp.Framework.Redis
{
    public static class RedisWrapperFactory
    {
        public static IRedisWrapper GetRedisWrapper(string redisConfigName = null)
        {
            if (redisConfigName.IsNullOrEmpty())
            {
                redisConfigName = CommonConfig.RedisConfigName;
            }

            return new StackExchangeRedisWrapper(redisConfigName);
        }
    }
}

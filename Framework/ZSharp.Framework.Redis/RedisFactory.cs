using ZSharp.Framework.Configurations;
using ZSharp.Framework.Extensions;

namespace ZSharp.Framework.Redis
{
    public static class RedisFactory
    {
        public static IRedisWrapper GetRedisWrapper(string redisConfigName = null)
        {
            redisConfigName = GetRedisConfigName(redisConfigName);
            return new StackExchangeRedisWrapper(redisConfigName);
        }

        public static IRedisLock GetRedisLock(string redisConfigName = null)
        {
            return new RedisLock(redisConfigName);
        }

        private static string GetRedisConfigName(string redisConfigName = null)
        {
            if (redisConfigName.IsNullOrEmpty())
            {
                redisConfigName = CommonConfig.RedisConfigName;
            }

            return redisConfigName;
        }
    }
}

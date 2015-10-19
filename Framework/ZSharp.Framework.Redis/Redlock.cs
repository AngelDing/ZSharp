using System;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.Redis
{
    public class Redlock : DisposableObject
    {
        private readonly IRedisLock redisLock;
        private string key;
        private long lockExpireTime;

        public Redlock(string key, TimeSpan timeOut)
        {
            this.redisLock = RedisFactory.GetRedisLock();
            this.key = key;
            CreateLockString(timeOut);
            ExecHelper.RetryUntilTrue(() => { return redisLock.Lock(key, lockExpireTime, timeOut); }, timeOut);
        }

        private void CreateLockString(TimeSpan timeOut)
        {
            var expireTime = DateTimeOffset.UtcNow.Add(timeOut);
            this.lockExpireTime = expireTime.ToUnixTimeMilliseconds() + 1;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                redisLock.UnLock(key, lockExpireTime.ToString());
            }
        }
    }
}

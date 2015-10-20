using System;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.Redis
{
    public class Redlock : DisposableObject
    {
        private readonly IRedisLock redisLock;
        private LockInfo lockInfo;

        public Redlock(string resource, TimeSpan timeOut)
        {
            this.redisLock = RedisFactory.GetRedisLock();
            this.lockInfo = GetLockInfo(resource, timeOut);
            ExecHelper.RetryUntilTrue(() => { return redisLock.Lock(lockInfo); }, timeOut);
        }

        private LockInfo GetLockInfo(string resource, TimeSpan timeOut)
        {
            var val = GuidHelper.NewSequentialId().ToString();
            return new LockInfo(resource, val, timeOut);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                redisLock.UnLock(lockInfo);
            }
        }
    }
}

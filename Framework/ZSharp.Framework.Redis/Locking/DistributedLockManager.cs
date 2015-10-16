
using System;

namespace ZSharp.Framework.Redis
{
    public class DistributedLockManager
    {
        public IDistributedLock GetDistributedLock()
        {
            return new StackExchangeRedisLock();
        }

        public static IDisposable AcquireLock(string key, TimeSpan? timeOut)
        {
            var locker = new DistributedLockManager().GetDistributedLock();
            return locker.AcquireLock(key, timeOut);
        }
    }
}

using System;

namespace ZSharp.Framework.Infrastructure
{
    public class DistributedLockFactory
    {
        public static IDisposable GetLock(string key, TimeSpan? timeOut)
        {
            var realTimeOut = timeOut ?? TimeSpan.FromSeconds(30);
            var locker = ServiceLocator.GetInstance<IDistributedLock>();
            return locker.GetLockObject(key, realTimeOut);
        }

        public static IDisposable GetLock(string key, int timeOutSeconds = 30)
        {
            var timeOut = TimeSpan.FromSeconds(timeOutSeconds);
            return GetLock(key, timeOut);
        }
    }
}

using System;
using ZSharp.Framework.Redis;

namespace ZSharp.Framework.Infrastructure
{
    /// <summary>
    /// 採用Redis實現的分佈式鎖，也可以採用其他實現方式
    /// </summary>
    public class DistributedLock : IDistributedLock
    {
        public IDisposable GetLockObject(string key, TimeSpan timeOut)
        {
            return new Redlock(key, timeOut);
        }
    }
}

using System;

namespace ZSharp.Framework.Redis
{
    public interface IDistributedLock : IDisposable
    {
        IDisposable AcquireLock(string key, TimeSpan? timeOut);
    }
}

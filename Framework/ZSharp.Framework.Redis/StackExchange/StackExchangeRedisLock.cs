using System;

namespace ZSharp.Framework.Redis
{
    public class StackExchangeRedisLock : DisposableObject, IDistributedLock
    {
        public IDisposable AcquireLock(string key, TimeSpan? timeOut)
        {
            throw new NotImplementedException();
        }

        private void Lock()
        {
        }

        private void UnLock()
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }
    }
}

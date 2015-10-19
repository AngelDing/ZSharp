using System;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.Redis
{
    public class Redlock : DisposableObject
    {
        private readonly IRedisLock redisLock;
        private readonly string key;

        public Redlock(string key, TimeSpan timeOut)
        {
            this.redisLock = RedisFactory.GetRedisLock();
            this.key = key;
            ExecHelper.RetryUntilTrue(() => { return redisLock.Lock(key, timeOut); }, timeOut);
        }      
         
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                redisLock.UnLock(key);
            }
        }
    }
}

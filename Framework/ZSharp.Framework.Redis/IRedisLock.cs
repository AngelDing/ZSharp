using System;

namespace ZSharp.Framework.Redis
{
    public interface IRedisLock
    {
        bool Lock(string key, long lockExpireTime, TimeSpan timeOut);

        void UnLock(string key, string value);
    }
}

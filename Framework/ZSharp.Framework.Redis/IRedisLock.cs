using System;

namespace ZSharp.Framework.Redis
{
    public interface IRedisLock
    {
        bool Lock(string key, TimeSpan timeOut);

        void UnLock(string key);
    }
}

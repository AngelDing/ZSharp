using System;

namespace ZSharp.Framework.Redis
{
    public interface IRedisLock
    {
        bool Lock(LockInfo lockInfo);

        void UnLock(LockInfo lockInfo);
    }
}

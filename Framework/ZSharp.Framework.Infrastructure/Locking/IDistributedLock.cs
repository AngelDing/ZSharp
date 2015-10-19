using System;

namespace ZSharp.Framework.Infrastructure
{
    public interface IDistributedLock
    {
        /// <summary>
        /// 獲取鎖定對象
        /// </summary>
        /// <param name="key">需要鎖定的資源Key</param>
        /// <param name="timeOut">超時時間</param>
        /// <returns>可以釋放的鎖定對象</returns>
        IDisposable GetLockObject(string key, TimeSpan timeOut);
    }
}

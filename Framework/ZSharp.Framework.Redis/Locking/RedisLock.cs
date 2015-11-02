using System.Collections.Generic;

namespace ZSharp.Framework.Redis
{
    public class RedisLock : IRedisLock
    {
        /// <summary>
        /// String containing the Lua unlock script.
        /// </summary>
        const string UnlockScript = @"
            if redis.call(""get"",KEYS[1]) == ARGV[1] then
                return redis.call(""del"",KEYS[1])
            else
                return 0
            end";

        private readonly IRedisWrapper redisWrapper;

        public RedisLock(string redisConfigName = null)
        {
            this.redisWrapper = RedisFactory.GetRedisWrapper(redisConfigName);
        }

        public bool Lock(LockInfo lockInfo)
        {
            var key = lockInfo.Resource;
            var val = lockInfo.Value;
            var ttl = lockInfo.TTL;
            var isOk = redisWrapper.SetIfNotExists(key, val, ttl);

            return isOk;
        }

        //public bool Lock(LockInfo lockInfo)
        //{
        //    var key = lockInfo.Resource;
        //    var timeOut = lockInfo.TTL;
        //    var lockExpireTime = CreateLockExpireTime(timeOut);
        //    var lockString = lockExpireTime.ToString();

        //    //當沒有正常釋放時，在原來過期時間基礎上加30天讓其會自動釋放；
        //    var autoReleaseSpan = timeOut.Add(TimeSpan.FromDays(30));

        //    //Try to set the lock, if it does not exist this will succeed and the lock is obtained
        //    var isOk = redisWrapper.SetIfNotExists(key, lockString, autoReleaseSpan);
        //    if (isOk)
        //    {
        //        return true;
        //    }

        //    //If we've gotten here then a key for the lock is present. This could be because the lock is
        //    //correctly acquired or it could be because a client that had acquired the lock crashed (or didn't release it properly).
        //    //Therefore we need to get the value of the lock to see when it should expire
        //    string lockExpireString = string.Empty;
        //    var tempValue = redisWrapper.Get(key);
        //    if (tempValue != null)
        //    {
        //        lockExpireString = tempValue.ToString();
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //    lockExpireTime = lockExpireString.ToLong(0);
        //    //If the expire time is greater than the current time then we can't let the lock go yet
        //    var unixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        //    if (lockExpireTime > unixMs)
        //    {
        //        return false;
        //    }

        //    //If the expire time is less than the current time then it wasn't released properly and we can attempt to 
        //    //acquire the lock. This is done by setting the lock to our timeout string AND checking to make sure
        //    //that what is returned is the old timeout string in order to account for a possible race condition.
        //    var oldString = redisWrapper.GetSet(key, lockString).ToString();
        //    return oldString == lockExpireString;
        //}

        //private long CreateLockExpireTime(TimeSpan timeOut)
        //{
        //    var expireTime = DateTimeOffset.UtcNow.Add(timeOut);

        //    return expireTime.ToUnixTimeMilliseconds() + 1;
        //}

        public void UnLock(LockInfo lockInfo)
        {
            var keys = new List<string> { lockInfo.Resource };
            var vals = new List<string> { lockInfo.Value };
            redisWrapper.ScriptEvaluate(UnlockScript, keys, vals);
        }
    }
}


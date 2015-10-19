using System;
using System.Collections.Generic;

namespace ZSharp.Framework.Redis
{
    public interface IRedisWrapper
    {
        object Get(string key);

        object GetAndSet(string key, string dataStr);

        void Set(string key, string dataStr, TimeSpan? expiry = null);

        bool SetIfNotExists(string key, string dataStr, TimeSpan? expiry = null);

        bool Exists(string key);

        void Remove(string key);

        void RemoveByPattern(string pattern);

        void ClearAll();

        void KeyExpire(string key, TimeSpan expirationTimeout);

        Dictionary<string, string> HashGetAll(string key);

        void HashSet(string key, IList<KeyValuePair<string, string>> hashItems);

        void HashDelete(string key, IList<string> dataItems);

        object ScriptEvaluate(string script, IEnumerable<string> keys = null, IEnumerable<string> values = null);
    }
}

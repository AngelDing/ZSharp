
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Timers;
namespace ZSharp.Framework.Redis
{
    public sealed class StackExchangeRedisWrapper : IRedisWrapper
    {
        private static IDatabase db = null;
        private readonly StackExchangeRedisFactory factory;
        private static Timer connMessagesSentTimer;

        public StackExchangeRedisWrapper(string redisConfigName)
        {
            factory = new StackExchangeRedisFactory(redisConfigName);
            db = factory.GetDatabase();
            connMessagesSentTimer = new Timer(30000);
            connMessagesSentTimer.Elapsed += StackExchangeRedisWrapper.GetConnectionsMessagesSent;
            connMessagesSentTimer.Start();
        }

        /// <summary>
        /// Gets the number of redis commands sent and received, and sets the count to 0 so the next time
        ///     we will not see double counts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void GetConnectionsMessagesSent(object sender, ElapsedEventArgs e)
        {
            //bool logCount = RedisConnectionConfig.LogConnectionActionsCountDel != null;

            //if (logCount)
            //{
            //    foreach (string connName in RedisConnectionWrapper.RedisConnections.Keys.ToList())
            //    {
            //        try
            //        {
            //            ConnectionMultiplexer conn;
            //            if (RedisConnectionWrapper.RedisConnections.TryGetValue(connName, out conn))
            //            {
            //                long priorPeriodCount = 0;
            //                if (RedisConnectionWrapper.RedisStats.ContainsKey(connName))
            //                {
            //                    priorPeriodCount = RedisConnectionWrapper.RedisStats[connName];
            //                }

            //                ServerCounters counts = conn.GetCounters();
            //                long curCount = counts.Interactive.OperationCount;

            //                // log the sent commands
            //                RedisConnectionConfig.LogConnectionActionsCountDel(
            //                    connName,
            //                    curCount - priorPeriodCount);

            //                RedisConnectionWrapper.RedisStats[connName] = curCount;
            //            }
            //        }
            //        catch (Exception)
            //        {
            //        }
            //    }
            //}
        }

        public object Get(string key)
        {
            var data = db.StringGet(key);
            if (data.IsNull)
            {
                return null;
            }
            return data;
        }

        public object GetSet(string key, string dataStr)
        {
            return db.StringGetSet(key, dataStr);
        }

        public void Set(string key, string dataStr, TimeSpan? expiry = null)
        {
            db.StringSet(key, dataStr, expiry);
        }

        public bool SetIfNotExists(string key, string dataStr, TimeSpan? expiry = null)
        {
            return db.StringSet(key, dataStr, expiry, When.NotExists);
        }

        public bool Exists(string key)
        {
            return db.KeyExists(key); 
        }

        public void Remove(string key)
        {
            db.KeyDelete(key, CommandFlags.FireAndForget);
        }

        public void RemoveByPattern(string pattern)
        {
            var keys = new List<RedisKey>();

            var endPoints = db.Multiplexer.GetEndPoints();

            foreach (var endpoint in endPoints)
            {
                var dbKeys = db.Multiplexer.GetServer(endpoint).Keys(pattern: pattern);

                foreach (var dbKey in dbKeys)
                {
                    if (!keys.Contains(dbKey))
                    {
                        keys.Add(dbKey);
                    }
                }
            }

            keys.ForEach(k => Remove(k));
        }

        public void ClearAll()
        {
            var endPoints = db.Multiplexer.GetEndPoints();

            foreach (var endpoint in endPoints)
            {
                var server = db.Multiplexer.GetServer(endpoint);
                if (!server.IsSlave)
                {
                    server.FlushDatabase(db.Database);
                }                
            }
        }

        public void KeyExpire(string key, TimeSpan expirationTimeout)
        {
            db.KeyExpire(key, expirationTimeout, CommandFlags.FireAndForget);
        }

        public Dictionary<string, string> HashGetAll(string key)
        {
            HashEntry[] redisHashData = db.HashGetAll(key);

            var redisData = new Dictionary<string, string>();
            foreach (var sessDataEntry in redisHashData)
            {
                string hashItemKey = sessDataEntry.Name.ToString();
                string hashItemValue = sessDataEntry.Value.ToString();
                redisData.Add(hashItemKey, hashItemValue);
            }
            return redisData;
        }

        public void HashSet(string key, IList<KeyValuePair<string, string>> hashItems)
        {
            var setItems = new List<HashEntry>();
            foreach (var item in hashItems)
            {
                var entry = new HashEntry(item.Key, item.Value);
                setItems.Add(entry);
            }

            db.HashSet(key, setItems.ToArray(), CommandFlags.FireAndForget);
        }

        public void HashDelete(string key, IList<string> dataItems)
        {
            var delItems = new List<RedisValue>();
            foreach (var item in dataItems)
            {
                delItems.Add(item);
            }
            db.HashDelete(key, delItems.ToArray(), CommandFlags.FireAndForget);
        }

        public object ScriptEvaluate(string script, IEnumerable<string> keyList = null, IEnumerable<string> valueList = null)
        {
            var keys = GetRedisKeys(keyList);
            var values = GetRedisValues(valueList);
            var result = db.ScriptEvaluate(script, keys, values);
            return result;
        }

        private RedisKey[] GetRedisKeys(IEnumerable<string> keyList = null)
        {
            if (keyList == null)
            {
                return null;
            }
            var redisKeys = new List<RedisKey>();
            foreach (var k in keyList)
            {
                redisKeys.Add(k);
            }
            return redisKeys.ToArray();
        }

        private RedisValue[] GetRedisValues(IEnumerable<string> valueList = null)
        {
            if (valueList == null)
            {
                return null;
            }

            var redisValues = new List<RedisValue>();
            foreach (var v in valueList)
            {
                redisValues.Add(v);
            }
            return redisValues.ToArray();
        }

        public long Increment(string key)
        {
            return db.StringIncrement(key);
        }
    }
}

using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.MongoDb.Managers
{
    public class IndexManager<T> : BaseMongoDB<T>, IIndexManager<T> where T : class
    {
        public IndexManager(string connectionStringOrName)
            : this(connectionStringOrName, null)
        {
        }

        public IndexManager(string connectionStringOrName, string collName)
            : base(connectionStringOrName, collName)
        {
        }

        /// <summary>
        /// 判斷是否存在指定的索引
        /// </summary>
        /// <param name="indexName">索引名稱</param>
        /// <returns>True/False</returns>
        public bool IndexExist(string keyName)
        {
            var indexList = new List<BsonDocument>();
            AsyncHelper.RunSync(() => this.Collection.Indexes.ListAsync()
                .Result.ForEachAsync(i => indexList.Add(i)));
            var exist = false;
            foreach (var index in indexList)
            {
                foreach (var e in index.Elements)
                {
                    if (e.Name == "name" && e.Value == keyName)
                    {
                        exist = true;
                        break;
                    }
                }
            }
            return exist;
        }

        /// <summary>
        /// 刪除某個字段的索引
        /// </summary>
        /// <param name="keyName">字段名稱</param>
        public void DropIndex(string keyName)
        {
            this.DropIndexes(new string[] { keyName });
        }

        /// <summary>
        /// 批量刪除字段的索引
        /// </summary>
        /// <param name="indexNames">索引名稱名稱</param>
        public void DropIndexes(IEnumerable<string> indexNames)
        {
            foreach (var name in indexNames)
            {
                this.DropIndexByName(name);
            }
        }

        /// <summary>
        /// 刪除指定索引名稱的索引，注意此索引可能是復合索引
        /// </summary>
        /// <param name="indexName">索引名稱</param>
        public void DropIndexByName(string indexName)
        {
            AsyncHelper.RunSync(() => this.Collection.Indexes.DropOneAsync(indexName));
        }

        /// <summary>
        /// 指定字段創建索引,索引名稱同字段名稱
        /// </summary>
        /// <param name="keyName">字段名稱</param>
        public void CreateIndex(string keyName)
        {
            this.CreateIndexes(new string[] { keyName }, keyName);
        }

        /// <summary>
        /// 創建符合索引，并指定索引名稱，所有字段均按順序排列
        /// </summary>
        /// <param name="keyNames">字段名稱結合</param>
        /// <param name="indexName">索引名稱</param>
        public void CreateIndexes(IEnumerable<string> keyNames, string indexName)
        {
            var newKeyNames = new List<IndexKey>();
            foreach (var key in keyNames)
            {
                var newKey = new IndexKey(key, IndexOrderType.Ascending);
                newKeyNames.Add(newKey);
            }
            this.CreateIndexes(newKeyNames, indexName);            
        }

        /// <summary>
        /// 創建符合索引，并指定索引名稱，所有字段均可按需指定排列順序
        /// </summary>
        /// <param name="keyNames">字段名稱結合</param>
        /// <param name="indexName">索引名稱</param>
        public void CreateIndexes(IEnumerable<IndexKey> keyNames, string indexName)
        {
            var builder = Builders<T>.IndexKeys;
            var indexKeys = new List<IndexKeysDefinition<T>>();
            foreach (var key in keyNames)
            {
                IndexKeysDefinition<T> indexKey;
                if (key.OrderType == IndexOrderType.Ascending)
                {
                    indexKey = builder.Ascending(key.FieldName);
                }
                else
                {
                    indexKey = builder.Descending(key.FieldName);
                }
                indexKeys.Add(indexKey);
            }

            var keyList = builder.Combine(indexKeys);
            var options = new CreateIndexOptions { Name = indexName };

            this.CreateIndexes(keyList, options);
        }

        /// <summary>
        /// Mongo原生驅動支持的創建索引方法，建議使用上面的接口，原生接口是異步執行，這裡改為同步執行
        /// </summary>
        /// <param name="keys">原生索引定義</param>
        /// <param name="options">相關索引設置，比如名稱，過期日期等</param>
        public void CreateIndexes(IndexKeysDefinition<T> keys, CreateIndexOptions options = null)
        {
            AsyncHelper.RunSync(() => this.Collection.Indexes.CreateOneAsync(keys, options));
        }
    }
}

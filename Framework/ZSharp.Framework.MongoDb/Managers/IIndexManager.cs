using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace ZSharp.Framework.MongoDb.Managers
{
    public interface IIndexManager<T>
        where T : class
    {
        /// <summary>
        /// 判斷是否存在指定的索引
        /// </summary>
        /// <param name="indexName">索引名稱</param>
        /// <returns>True/False</returns>
        bool IndexExist(string indexName);

        /// <summary>
        /// 刪除某個字段的索引
        /// </summary>
        /// <param name="keyName">字段名稱</param>
        void DropIndex(string keyName);

        /// <summary>
        /// 刪除指定索引名稱的索引，注意此索引可能是復合索引
        /// </summary>
        /// <param name="indexName">索引名稱</param>
        void DropIndexByName(string indexName);

        /// <summary>
        /// 批量刪除字段的索引
        /// </summary>
        /// <param name="keyNames">字段名稱</param>
        void DropIndexes(IEnumerable<string> indexNames);

        /// <summary>
        /// 指定字段創建索引,索引名稱同字段名稱
        /// </summary>
        /// <param name="keyName">字段名稱</param>
        void CreateIndex(string keyName);

        /// <summary>
        /// 創建符合索引，并指定索引名稱，所有字段均按順序排列
        /// </summary>
        /// <param name="keyNames">字段名稱結合</param>
        /// <param name="indexName">索引名稱</param>
        void CreateIndexes(IEnumerable<string> keyNames, string indexName);

        /// <summary>
        /// 創建符合索引，并指定索引名稱，所有字段均可按需指定排列順序
        /// </summary>
        /// <param name="keyNames">字段名稱結合</param>
        /// <param name="indexName">索引名稱</param>
        void CreateIndexes(IEnumerable<IndexKey> keyNames, string indexName);

        /// <summary>
        /// Mongo原生驅動支持的創建索引方法，建議使用上面的接口
        /// </summary>
        /// <param name="keys">原生索引定義</param>
        /// <param name="options">相關索引設置，比如名稱，過期日期等</param>
        void CreateIndexes(IndexKeysDefinition<T> keys, CreateIndexOptions options = null);
    }
}

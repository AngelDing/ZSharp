
namespace ZSharp.Framework.MongoDb.Managers
{
    public enum TreeNodeType
    {
        /// <summary>
        /// 服务器
        /// </summary>
        Server = 1,
        /// <summary>
        /// 数据库
        /// </summary>
        Database = 2,
        /// <summary>
        /// 数据表
        /// </summary>
        Collection = 3,
        /// <summary>
        /// 字段
        /// </summary>
        Field = 4,
        /// <summary>
        /// 索引
        /// </summary>
        Index = 5,
        /// <summary>
        /// 表信息填充节点
        /// </summary>
        TableFiller = 6,
        /// <summary>
        /// 索引填充节点
        /// </summary>
        IndexFiller = 7
    }
}

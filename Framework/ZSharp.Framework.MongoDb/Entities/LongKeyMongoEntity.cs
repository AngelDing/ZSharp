
using ZSharp.Framework.Entities;

namespace ZSharp.Framework.MongoDb
{
    /// <summary>
    /// 不推薦使用，推薦使用objectId/guid作為主鍵
    /// </summary>
    public class LongKeyMongoEntity : BaseMongoEntity, IAggregateRoot<long>
    {
        public long Id { get; set; }
    }
}

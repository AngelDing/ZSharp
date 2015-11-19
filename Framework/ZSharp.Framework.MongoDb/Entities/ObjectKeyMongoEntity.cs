using MongoDB.Bson;
using ZSharp.Framework.Entities;

namespace ZSharp.Framework.MongoDb
{
    public class ObjectKeyMongoEntity : Entity, IAggregateRoot<ObjectId>
    {
        public ObjectId Id { get; set; }

        //TODO:需要重寫對象的Equals方法
    }
 
}

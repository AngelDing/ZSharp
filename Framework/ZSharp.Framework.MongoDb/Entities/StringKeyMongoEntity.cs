using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ZSharp.Framework.Entities;

namespace ZSharp.Framework.MongoDb
{
    public class StringKeyMongoEntity : Entity, IAggregateRoot<string>
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        //TODO:需要重寫對象的Equals方法
    }
}

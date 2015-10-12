using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ZSharp.Framework.Entities;

namespace ZSharp.Framework.MongoDb
{
    public class StringKeyMongoEntity : BaseMongoEntity, IAggregateRoot<string>
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}

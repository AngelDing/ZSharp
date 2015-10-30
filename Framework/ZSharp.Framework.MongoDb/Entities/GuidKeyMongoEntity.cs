using System;
using ZSharp.Framework.Entities;

namespace ZSharp.Framework.MongoDb
{
    public class GuidKeyMongoEntity : MongoEntity, IAggregateRoot<Guid>
    {
        public Guid Id { get; set; }
    }
}

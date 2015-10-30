using System;
using ZSharp.Framework.Entities;
using ZSharp.Framework.MongoDb;
using ZSharp.Framework.MongoDb.Managers;

namespace Framework.MongoDb.Test
{
    public class MongoTestDB<T, TKey> : MongoRepository<T, TKey> where T : MongoEntity, IAggregateRoot<TKey>
    {
        public MongoTestDB()
            : base("MongoTestDB")
        {
        }
    }

    public class MongoTestDB<T> : MongoTestDB<T, string> where T : StringKeyMongoEntity
    {
    }

    public class MongoIndexManagerTest<T> : IndexManager<T> where T : MongoEntity
    {
        public MongoIndexManagerTest()
            : base("MongoTestDB")
        {
        }

        private static string GetCollectionName()
        {
            string collectionName;
            var att = Attribute.GetCustomAttribute(typeof(T), typeof(CollectionNameAttribute));
            if (att != null)
            {
                collectionName = ((CollectionNameAttribute)att).Name;
            }
            else
            {
                collectionName = typeof(T).Name;
            }

            if (string.IsNullOrEmpty(collectionName))
            {
                throw new ArgumentException("Collection name cannot be empty for this entity");
            }
            return collectionName;
        }
    }
}

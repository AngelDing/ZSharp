using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Linq;

namespace ZSharp.Framework.MongoDb.IdGenerators
{
    public class LongIdGenerator<TDocument, TKey> : IIdGenerator where TDocument : class
    {
        private readonly object lockObject = new object();
        private static LongIdGenerator<TDocument, TKey> _instance = new LongIdGenerator<TDocument, TKey>();

        public static LongIdGenerator<TDocument, TKey> Instance { get { return _instance; } }

        public object GenerateId(object container, object document)
        {
            TKey id = default(TKey);

            var mongoDB = GetMongoDatabase(container);
            var idColl = mongoDB.GetCollection<IdentityEntity<TKey>>("IdentityEntity");
            var keyName = document.GetType().FullName;
            id = RealGenerateId(idColl, keyName);

            return id;
        }

        private static IMongoDatabase GetMongoDatabase(object container)
        {
            Type type = container.GetType();
            var property = from pi in type.GetProperties()
                           where pi.Name == "Database"
                           select pi;

            var mongoDB = property.First().GetValue(container, null) as IMongoDatabase;
            return mongoDB;
        }

        private TKey RealGenerateId(IMongoCollection<IdentityEntity<TKey>> idColl, string keyName)
        {
            TKey id;

            var idBuilder = Builders<IdentityEntity<TKey>>.Update.Inc("Value", 1);

            var options = new FindOneAndUpdateOptions<IdentityEntity<TKey>>
            {
                ReturnDocument = ReturnDocument.After,
                IsUpsert = true
            };

            var result = idColl.FindOneAndUpdateAsync<IdentityEntity<TKey>>(i => i.Key == keyName, idBuilder, options);

            id = result.Result.Value;

            return id;
        }        

        public bool IsEmpty(object id)
        {
            if (null == id)
            {
                return false;
            }

            return true;
        }
    }
}

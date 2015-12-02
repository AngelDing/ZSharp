using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Configuration;
using ZSharp.Framework.MongoDb;
using ZSharp.Framework.Utils;

namespace Framework.MongoDb.Test
{
    public abstract class BaseMongoTest : IDisposable
    {
        public BaseMongoTest()
        {
            MongoInitHelper.InitMongoDBRepository();

            //如果手动指定关系，则需要同时指定自定义的特殊序列化机制，否则会采用默认的序列化机制
            var dateTimeSerializer = new DateTimeSerializer(DateTimeKind.Local);
            var nullableDateTimeSerializer = new NullableSerializer<DateTime>(dateTimeSerializer);
            BsonClassMap.RegisterClassMap<MyTestEntity>(rc =>
            {
                rc.MapProperty(i => i.A);
                rc.MapProperty(i => i.B).SetSerializer(dateTimeSerializer);
                rc.MapProperty(i => i.C);
                rc.MapProperty(i => i.D).SetSerializer(nullableDateTimeSerializer);
            });
        }

        private void DropDatabase()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MongoTestDB"].ConnectionString;
            var url = new MongoUrl(connectionString);
            var client = new MongoClient(url);
            AsyncHelper.RunSync(() => client.DropDatabaseAsync(url.DatabaseName));
        }

        public virtual void Dispose()
        {
        }
    }
}

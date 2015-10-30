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

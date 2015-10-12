using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace ZSharp.Framework.MongoDb.Managers
{
    public interface IMongoManager
    {
        CommandResult GetServerInfo();

        CommandResult GetDatabaseInfo();

        CommandResult GetDatabaseList();

        CommandResult GetReplicationInfo();

        CommandResult GetCollectionInfo();

        IMongoCollection<BsonDocument> GetCollection(string collectionName);

        IList<BsonDocument> GetCollectionIndexs(string collName, string nameSpace);

        IEnumerable<string> GetCollectionNames();

        //GetProfilingLevelResult GetProfilingLevel();

        //MongoCursor<SystemProfileInfo> GetProfilingInfo(IMongoQuery query, int limit);

        //CommandResult SetProfilingLevel(ProfilingLevel level, TimeSpan timeSpan);

        //CommandResult SetProfilingLevel(ProfilingLevel level);
    }
}

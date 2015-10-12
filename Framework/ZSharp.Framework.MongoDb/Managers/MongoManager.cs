using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace ZSharp.Framework.MongoDb.Managers
{
    public class MongoManager : BaseMongoDB<BsonDocument>, IMongoManager
    {
        public MongoManager(string sName)
            : this(sName, ConstHelper.AdminDBName)
        {
        }

        public MongoManager(string sName, string dbName)
            : this(sName, dbName, null)
        {
        }

        public MongoManager(string sName, string dbName, string cName)
            : base(string.Format(ConstHelper.ConnString, sName, dbName), cName)
        {
        }

        public CommandResult GetServerInfo()
        {
            var command = new BsonDocument("serverStatus", 1);
            return RealExecuteCommand(command);
        }      

        public CommandResult GetDatabaseInfo()
        {
            var command = new BsonDocument("dbstats", 1);
            return RealExecuteCommand(command);  
        }

        public CommandResult GetDatabaseList()
        {
            var command = new BsonDocument("listDatabases", 1);
            return RealExecuteCommand(command);  
        }

        public CommandResult GetCollectionInfo()
        {
            var command = new BsonDocument("collstats", this.Collection.CollectionNamespace.CollectionName);
            return RealExecuteCommand(command);  
        }

        public IMongoCollection<BsonDocument> GetCollection(string collName)
        {
            return DB.GetCollection<BsonDocument>(collName);
        }

        public IList<BsonDocument> GetCollectionIndexs(string collName, string nameSpace)
        {
            var coll = GetCollection(collName);
            var result = coll.Find(new BsonDocument { { "ns", nameSpace } });
            return result.ToListAsync().Result;
        }

        public IEnumerable<string> GetCollectionNames()
        {
            var collections = DB.ListCollectionsAsync().Result.ToListAsync().Result;
            var colStrList = new List<string>();
            foreach (var col in collections)
            {
                colStrList.Add(col["name"].ToString());
            }
            return colStrList;
        }

        //public GetProfilingLevelResult GetProfilingLevel()
        //{
        //    return DB.GetProfilingLevel();
        //}

        //public MongoCursor<SystemProfileInfo> GetProfilingInfo(IMongoQuery query, int limit)
        //{
        //    return DB.GetProfilingInfo(query).SetLimit(limit);
        //}

        //public CommandResult SetProfilingLevel(ProfilingLevel level, TimeSpan timeSpan)
        //{
        //    return DB.SetProfilingLevel(level, timeSpan);
        //}

        //public CommandResult SetProfilingLevel(ProfilingLevel level)
        //{
        //    return DB.SetProfilingLevel(level);
        //}


        public CommandResult GetReplicationInfo()
        {
            var command = new BsonDocument("ismaster", 1);
            return RealExecuteCommand(command);  
        }

        private CommandResult RealExecuteCommand(BsonDocument command)
        {
            var response = DB.RunCommandAsync<BsonDocument>(command).Result;
            return new CommandResult(response);
        }
    }
}

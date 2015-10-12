//using MongoDB.Bson;
//using System.Collections;
//using System.Collections.Generic;

//namespace ZSharp.Framework.MongoDb.Managers
//{
//    public class ReplicationContext : BaseContext
//    {
//        public ServerModel Server { get; private set; }
//        public ReplicationContext(int id)
//        {
//            Id = id;
//            var serverNode = CacheHelper.GetTreeNode(id);
//            Server = serverNode.ModelInfo as ServerModel;
//        }

//        public Hashtable GetReplicationInfo()
//        {
//            var hash = new Hashtable();
//            var manager = new MongoManager(Server.Name, ConstHelper.AdminDBName);

//            var stats = manager.GetReplicationInfo();

//            var serverInfo = new List<TreeNode>();
//            var dataInfo = new List<TreeNode>();
//            if (stats.Ok)
//            {
//                BuildTreeNode(serverInfo, 0, stats.Response);
//                hash.Add(0, serverInfo);

//                var localDB = new MongoManager(Server.Name, ConstHelper.LocalDBName);
//                if (stats.Response["ismaster"].AsBoolean)
//                {
//                    #region 日志信息
//                    var docs = localDB.GetCollection(ConstHelper.OplogTableName).FindAll().SetLimit(10);
//                    var doc = new BsonDocument();
//                    var idx = 0;
//                    foreach (var d in docs)
//                    {
//                        idx++;
//                        doc.Add("日志 No." + idx, d);
//                    }
//                    BuildTreeNode(dataInfo, 0, doc);
//                    #endregion
//                }
//                else
//                {
//                    #region 源服务器信息
//                    var doc = localDB.GetCollection(ConstHelper.SourceTableName).FindOne();
//                    BuildTreeNode(dataInfo, 0, doc);
//                    #endregion
//                }
//                hash.Add(1, dataInfo);
//            }
//            return hash;
//        }
//    }
//}

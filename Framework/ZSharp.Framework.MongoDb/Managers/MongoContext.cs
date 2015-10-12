using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ZSharp.Framework.MongoDb.Managers
{
    public class MongoContext
    {
        public ConcurrentBag<TreeNode> TreeNodes { get; set; }

        public MongoContext()
        {
            TreeNodes = new ConcurrentBag<TreeNode>();
            GetServer();
        }

        private void GetServer()
        {
            var servers = new ServerContext().GetServerNodes();            
            foreach (var s in servers)
            {
                var node = new TreeNode
                {
                    Id = s.Id,
                    PId = 0,
                    Name = s.Name,
                    Type = TreeNodeType.Server,
                    ModelInfo = s
                };
                TreeNodes.Add(node);
            }
            Parallel.ForEach(servers, s => GetDatabase(s.Id));
        }

        private void GetDatabase(int sId)
        {
            var serverNode = TreeNodes.FirstOrDefault(p => p.Id == sId && p.Type == TreeNodeType.Server);
            if (serverNode != null)
            {
                var manager = new MongoManager(serverNode.Name);
                var result = manager.GetDatabaseList();
                if (result.Ok)
                {
                    var dbDoc = result.Response;
                    var serverModel = serverNode.ModelInfo as ServerModel;
                    if (serverModel != null)
                    {
                        serverModel.IsOK = true;
                        serverModel.TotalSize = dbDoc["totalSize"].AsDouble;
                    }
                    var dbList = dbDoc["databases"].AsBsonArray;
                    if (dbList != null)
                    {
                        var tempDBList = new List<DatabaseModel>();
                        dbList.ToList().ForEach(item =>
                        {
                            var dbModel = new DatabaseModel
                            {
                                Server = serverModel,
                                Id = ConstHelper.GetRandomId(),
                                Name = item["name"].AsString,
                                Size = item["sizeOnDisk"].AsDouble
                            };
                            tempDBList.Add(dbModel);
                            TreeNodes.Add(new TreeNode
                            {
                                Id = dbModel.Id,
                                PId = serverModel.Id,
                                Name = dbModel.Name,
                                Type = TreeNodeType.Database,
                                ModelInfo = dbModel
                            });
                        });
                        Parallel.ForEach(tempDBList, d => GetCollection(d.Id));
                        //tempDBList.ForEach(d => GetCollection(d.Id));
                    }
                }
            }
        }

        private void GetCollection(int dbId)
        {
            var dbNode = TreeNodes.FirstOrDefault(p => p.Id == dbId && p.Type == TreeNodeType.Database);
            if (dbNode != null)
            {
                var dbModel = dbNode.ModelInfo as DatabaseModel;
                var manager = new MongoManager(dbModel.Server.Name, dbModel.Name);
                var result = manager.GetCollectionNames();
                if (result != null)
                {
                    var cList = result.Where(t => !t.Contains("$")
                        && !t.Contains(ConstHelper.IndexTableName)
                        && !t.Contains(ConstHelper.ProfileTableName))
                        .ToList();
                    var tempCollList = new List<CollectionModel>();
                    cList.ForEach(cName =>
                    {
                        var coll = manager.GetCollection(cName);
                        var collModel = new CollectionModel
                        {
                            Database = dbModel,
                            Id = ConstHelper.GetRandomId(),
                            Name = cName,
                            Namespace = coll.CollectionNamespace.FullName,
                            TotalCount = coll.CountAsync(new BsonDocument()).Result 
                        };
                        tempCollList.Add(collModel);
                        TreeNodes.Add(new TreeNode 
                        {
                            Id = collModel.Id,
                            PId = dbModel.Id,
                            Name = string.Format("{0} ({1})", collModel.Name, collModel.TotalCount),
                            Type = TreeNodeType.Collection,
                            ModelInfo = collModel
                        });
                    });
                    Parallel.ForEach(tempCollList, c => GetFieldAndIndex(c.Id));
                    //tempCollList.ForEach(c => GetFieldAndIndex(c.Id));
                }
            }
        }

        private void GetFieldAndIndex(int cId)
        {
            var collNode = TreeNodes.FirstOrDefault(p => p.Id == cId && p.Type == TreeNodeType.Collection);
            if (collNode != null)
            {
                var collModel = collNode.ModelInfo as CollectionModel;
                var dbModel = collModel.Database;
                var manager = new MongoManager(dbModel.Server.Name, dbModel.Name, collModel.Name);
                //字段
                GetField(collModel, manager);
                //索引
                GetIndex(collModel, manager);
            }
        }

        private void GetIndex(CollectionModel collModel, MongoManager manager)
        {
            //索引类型信息节点
            var indexNode = new TreeNode
            {
                Id = ConstHelper.GetRandomId(),
                PId = collModel.Id,
                Name = "索引",
                Type = TreeNodeType.IndexFiller
            };
            TreeNodes.Add(indexNode);

            //索引节点
            var indexes = manager.GetCollectionIndexs(ConstHelper.IndexTableName, collModel.Namespace);
            if (indexes != null)
            {
                foreach (var idx in indexes.ToList())
                {
                    var indexModel = new IndexModel
                    {
                        Id = ConstHelper.GetRandomId(),
                        Name = idx["name"].AsString,
                        Namespace = idx["ns"].AsString,
                        Unique = idx.Contains("unique") ? idx["unique"].AsBoolean : false,
                        Keys = new List<IndexKey>(),
                        Collection = collModel
                    };
                    var docFields = idx["key"].AsBsonDocument;
                    foreach (var key in docFields.Names)
                    {
                        var type = int.Parse(docFields[key.ToString()].ToString());
                        indexModel.Keys.Add(new IndexKey
                        {
                            FieldName = key.ToString(),
                            OrderType = type == 1 ? IndexOrderType.Ascending : IndexOrderType.Descending
                        });
                    }

                    TreeNodes.Add(new TreeNode
                    {
                        Id = indexModel.Id,
                        PId = indexNode.Id,
                        Name = indexModel.Name,
                        Type = TreeNodeType.Index,
                        ModelInfo = indexModel
                    });
                }
            }
        }

        private void GetField(CollectionModel collModel, MongoManager manager)
        {
            //字段类型信息节点
            var fieldNode = new TreeNode
            {
                Id = ConstHelper.GetRandomId(),
                PId = collModel.Id,
                Name = "表信息",
                Type = TreeNodeType.TableFiller
            };
            TreeNodes.Add(fieldNode);

            //字段节点
            var doc = manager.GetCollection(collModel.Name).Find(new BsonDocument())
                .Skip(1).FirstOrDefaultAsync().Result;
            if (doc != null)
            {
                foreach (var item in doc.Names)
                {
                    var fieldModel = new FieldModel
                    {
                        Id = ConstHelper.GetRandomId(),
                        Name = item.ToString(),
                        Type = doc[item].BsonType,
                        Collection = collModel
                    };

                    TreeNodes.Add(new TreeNode
                    {
                        Id = fieldModel.Id,
                        PId = fieldNode.Id,
                        Name = fieldModel.Name + string.Format(" ({0})", fieldModel.Type),
                        Type = TreeNodeType.Field,
                        ModelInfo = fieldModel
                    });
                }
            }
        }
    }
}

//using System.Linq;
//using System.Collections.Generic;
//using MongoDB.Bson;
//using MongoDB.Driver;

//namespace ZSharp.Framework.MongoDb.Managers
//{
//    public class DataContext : BaseContext
//    {
//        public CollectionModel Collection { get; private set; }

//        public DataContext(int id)
//        {
//            Id = id;
//            var collNode = CacheHelper.GetParentTreeNode(Id);
//            Collection = collNode.ModelInfo as CollectionModel;
//        }

//        public List<FieldModel> GetFields()
//        {
//            var tableFillerNode = CacheHelper.GetTreeNodes().FirstOrDefault(n => n.PId == Collection.Id 
//                && n.Type == TreeNodeType.TableFiller);
//            var fieldNodes = CacheHelper.GetTreeNodes().Where(p => p.PId == tableFillerNode.Id).ToList();

//            var list = new List<FieldModel>();
//            foreach (var node in fieldNodes)
//            {
//                list.Add(node.ModelInfo as FieldModel);
//            }
//            return list;
//        }

//        public List<BsonDocument> GetData(string jsonfind, string jsonsort, int skip, int limit)
//        {
//            var query = GetQuery(jsonfind, jsonsort);
//            if (skip > 0)
//            {
//                query.Skip = skip;
//            }
//            if (limit > 0)
//            {
//                query.Limit = limit;
//            }
//            return query.ToList();
//        }

//        public List<TreeNode> Explain(string jsonfind, string jsonsort)
//        {
//            var query = GetQuery(jsonfind, jsonsort);
//            var doc = query.Explain(true);

//            var list = new List<TreeNode>();
//            BuildTreeNode(list, 0, doc);
//            return list;
//        }

//        private MongoCursor<BsonDocument> GetQuery(string jsonfind, string jsonsort)
//        {
//            var sName = Collection.Database.Server.Name;
//            var dbName = Collection.Database.Name;
//            var manager = new MongoManager(sName, dbName);

//            var findDoc = string.IsNullOrEmpty(jsonfind)
//                ? new QueryDocument()
//                : new QueryDocument(BsonDocument.Parse(jsonfind));
//            var sortDoc = string.IsNullOrEmpty(jsonsort)
//                ? new SortByDocument()
//                : new SortByDocument(BsonDocument.Parse(jsonsort));

//            var query = manager.GetCollection(Collection.Name).Find(findDoc);
//            query.SetSortOrder(sortDoc);
//            return query;
//        }        
//    }
//}

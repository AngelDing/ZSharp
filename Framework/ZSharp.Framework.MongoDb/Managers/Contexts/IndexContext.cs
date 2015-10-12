using System.Linq;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ZSharp.Framework.MongoDb.Managers
{
    public class IndexContext : BaseContext
    {
        private string sName;
        private string dbName;
        private string collName;
        public CollectionModel Collection { get; private set; }

        public IndexContext(int id)
        {
            Id = id;
            var collNode = CacheHelper.GetParentTreeNode(Id);
            Collection = collNode.ModelInfo as CollectionModel;
            sName = Collection.Database.Server.Name;
            dbName = Collection.Database.Name;
            collName = Collection.Name;
        }

        public List<TreeNode> GetFieldNodes()
        {
            var fieldFiller = CacheHelper.GetTreeNodes()
                .Where(n => n.PId == Collection.Id && n.Type == TreeNodeType.TableFiller)
                .First();
            return CacheHelper.GetTreeNodes().Where(n => n.PId == fieldFiller.Id).ToList();
        }

        public List<IndexModel> GetIndexes()
        {
            var nodeList = CacheHelper.GetTreeNodes();
            var indexFiller = nodeList.FirstOrDefault(p => 
                p.PId == Collection.Id && p.Type == TreeNodeType.IndexFiller);
            var list = nodeList.Where(node => node.PId == indexFiller.Id).ToList();

            var indexes = new List<IndexModel>();
            foreach (var item in list)
            {
                indexes.Add(item.ModelInfo as IndexModel);
            }
            return indexes;
        }

        public void CreateIndex(string jsonData)
        {
            //var model = JsonConvert.DeserializeObject<IndexEditModel>(jsonData);
            //var idxDoc = ToDoc(model.Keys);
            //var idxOption = new IndexOptionsBuilder()
            //    .SetBackground(model.Background)
            //    .SetDropDups(model.DropDups)
            //    .SetSparse(model.Sparse)
            //    .SetName(model.IndexName);

            //var connStr = string.Format(ConstHelper.ConnString, sName, dbName);
            //var indexManager = new IndexManager<BsonDocument>(connStr, collName);
            //indexManager.CreateIndexes(idxDoc, idxOption);

            //CacheHelper.Clear();
        }

        //private IndexKeysDocument ToDoc(List<IndexKey> keys)
        //{
        //    var doc = new IndexKeysDocument();
        //    if (keys != null && keys.Count > 0)
        //    {
        //        foreach (var key in keys)
        //        {
        //            doc.Add(key.FieldName, (int)key.OrderType);
        //        }
        //    }
        //    return doc;
        //}

        public void DeleteIndex(int id)
        {         
            var idx = CacheHelper.GetTreeNode(id).ModelInfo as IndexModel;
            var connStr = string.Format(ConstHelper.ConnString, sName, dbName);
            var indexManager = new IndexManager<BsonDocument>(connStr, collName);

            indexManager.DropIndexByName(idx.Name);

            CacheHelper.Clear();
        }
    }
}

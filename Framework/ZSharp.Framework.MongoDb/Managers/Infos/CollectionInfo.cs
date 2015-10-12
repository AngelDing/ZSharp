using System;
using System.Collections.Generic;

namespace ZSharp.Framework.MongoDb.Managers
{
    public class CollectionInfo : BaseInfo
    {
        private string sName;
        private string dbName;
        private string collName;
        public CollectionInfo(int id)
        {
            var collNode = CacheHelper.GetTreeNode(id);
            var collModel = collNode.ModelInfo as CollectionModel;
            sName = collModel.Database.Server.Name;
            dbName = collModel.Database.Name;
            collName = collModel.Name;
        }

        public override List<TreeNode> GetInfo()
        {
            var manager = new MongoManager(sName, dbName, collName);
            var rst = manager.GetCollectionInfo();

            var list = new List<TreeNode>();
            if (rst.Ok)
            {
                BuildTreeNode(list, 0, rst.Response);
            }
            return list;
        }
    }
}
using System;
using System.Collections.Generic;

namespace ZSharp.Framework.MongoDb.Managers
{
    public class DatabaseInfo : BaseInfo
    {
        private string sName;
        private string dbName;
        public DatabaseInfo(int id)
        {
            var dbNode = CacheHelper.GetTreeNode(id);
            var dbModel = dbNode.ModelInfo as DatabaseModel;
            sName = dbModel.Server.Name;
            dbName = dbModel.Name;
        }

        public override List<TreeNode> GetInfo()
        {
            var manager = new MongoManager(sName, dbName);
            var rst = manager.GetDatabaseInfo();

            var list = new List<TreeNode>();
            if (rst.Ok)
            {
                BuildTreeNode(list, 0, rst.Response);
            }
            return list;
        }
    }
}
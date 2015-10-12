using System;
using System.Collections.Generic;

namespace ZSharp.Framework.MongoDb.Managers
{
    public class ServerInfo : BaseInfo
    {
        private string sName;
        public ServerInfo(int id)
        {
            var sNode = CacheHelper.GetTreeNode(id);
            var sModel = sNode.ModelInfo as ServerModel;
            sName = sModel.Name;
        }

        public override List<TreeNode> GetInfo()
        {
            var manager = new MongoManager(sName, ConstHelper.AdminDBName);
            var rst = manager.GetServerInfo();

            var list = new List<TreeNode>();
            if (rst.Ok)
            {
                BuildTreeNode(list, 0, rst.Response);
            }
            return list;
        }
    }
}
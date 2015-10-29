using System.Collections.Generic;
using System.Linq;
//using ZSharp.Framework.Caching;

namespace ZSharp.Framework.MongoDb.Managers
{
    public static class CacheHelper
    {
        public static readonly string NodeKey = "Cache_Tree_Nodes";

        public static List<TreeNode> GetTreeNodes()
        {
            return null;
            //var expDate = DateTimeOffset.Now.AddHours(2);
            //var cachePolicy = CachePolicy.WithAbsoluteExpiration(expDate);
            //var cacheManager = ZSharp.Framework.Caching.CacheHelper.MemoryCache;
            //var nodes = cacheManager.Get<List<TreeNode>>(NodeKey, () => RealGetTreeNodes(), cachePolicy);
            //return nodes;
        }

        private static List<TreeNode> RealGetTreeNodes()
        {
            var context = new MongoContext();
            var nodes = context.TreeNodes.ToList();
            return nodes;
        }

        public static TreeNode GetTreeNode(int id)
        {
            var nodes = GetTreeNodes();
            return nodes.Single(i => i.Id == id);
        }

        public static TreeNode GetParentTreeNode(int id)
        {
            var node = GetTreeNode(id);
            return GetTreeNode(node.PId);
        }

        public static IModel GetMongoModelInfo(int id)
        {
            IModel model = null;
            var node = GetTreeNode(id);
            if (node != null)
            {
                model = node.ModelInfo;
            }
            return model;
        }

        public static void Clear()
        {
            //ZSharp.Framework.Caching.CacheHelper.MemoryCache.Remove(NodeKey);
        }
    }
}

using System.Collections.Generic;

namespace ZSharp.Framework.MongoDb.Managers
{
    public class TreeNode
    {
        public int Id { get; set; }

        public int PId { get; set; }

        public string Name { get; set; }

        public TreeNodeType Type { get; set; }

        public IModel ModelInfo { get; set; }
    }
}

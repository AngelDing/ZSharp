using System.Collections.Generic;

namespace ZSharp.Framework.MongoDb.Managers
{
    public class BaseInfo : BaseContext, IMongoInfo
    {
        public virtual List<TreeNode> GetInfo()
        {
            return null;
        }
    }
}
using System.Collections.Generic;

namespace ZSharp.Framework.MongoDb.Managers
{
    public class MongoInfoFactory : IMongoInfo
    {
        private IMongoInfo MongoInfo { get; set; }

        public static MongoInfoFactory Create(int id, int type)
        {
            var factory = new MongoInfoFactory();
            if (type == (int)TreeNodeType.Server)
            {
                factory.MongoInfo = new ServerInfo(id);
            }
            else if (type == (int)TreeNodeType.Database)
            {
                factory.MongoInfo = new DatabaseInfo(id);
            }
            else if (type == (int)TreeNodeType.Collection)
            {
                factory.MongoInfo = new CollectionInfo(id);
            }
            return factory;
        }

        /// <summary>
        /// 获取服务器信息
        /// </summary>
        /// <returns></returns>
        public List<TreeNode> GetInfo()
        {
            return MongoInfo.GetInfo();
        }
    }
}

using MongoDB.Bson;
using System.Collections.Generic;

namespace ZSharp.Framework.MongoDb.Managers
{
    public class BaseContext
    {
        internal int Id { get; set; }

        internal void BuildTreeNode(List<TreeNode> list, int pId, BsonDocument doc)
        {
            foreach (var key in doc.Names)
            {
                var node = new TreeNode
                {
                    Id = ConstHelper.GetRandomId(),
                    PId = pId
                };

                var value = doc[key.ToString()];
                if (value is BsonDocument)
                {
                    node.Name = key.ToString();
                    list.Add(node);
                    BuildTreeNode(list, node.Id, value as BsonDocument);
                }
                else
                {
                    node.Name = string.Format("{0} : {1}", key, doc[key.ToString()]);
                    list.Add(node);
                }
            }
        }
    }
}

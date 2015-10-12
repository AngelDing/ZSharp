using System.Collections.Generic;

namespace ZSharp.Framework.MongoDb.Managers
{
    public class IndexModel : BaseModel, IModel
    {
        public CollectionModel Collection { get; set; }

        public string Namespace { get; set; }

        public List<IndexKey> Keys { get; set; }

        public bool Unique { get; set; }
    }

    public class IndexKey
    {
        public IndexKey()
        { 
        }

        public IndexKey(string fieldName, IndexOrderType orderType)
        {
            this.FieldName = fieldName;
            this.OrderType = orderType;
        }

        public string FieldName { get; set; }

        public IndexOrderType OrderType { get; set; }
    }
}

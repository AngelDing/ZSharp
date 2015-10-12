using System.Collections.Generic;

namespace ZSharp.Framework.MongoDb.Managers
{
    public class IndexEditModel
    {
        public string IndexName { get; set; }

        public List<IndexKey> Keys { get; set; }

        public bool Unique { get; set; }

        public bool Background { get; set; }

        public bool DropDups { get; set; }

        public bool Sparse { get; set; }
    }
}

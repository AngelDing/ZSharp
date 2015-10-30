using ZSharp.Framework.MongoDb;

namespace Framework.MongoDb.Test
{
    public class Product : StringKeyMongoEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
    }
}

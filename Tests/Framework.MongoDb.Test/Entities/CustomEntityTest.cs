using ZSharp.Framework.MongoDb;

namespace Framework.MongoDb.Test
{
    public class CustomEntityTest : IntKeyMongoEntity
    {
        public string Name { get; set; }
    }

    public class CustomEntityTest2 : IntKeyMongoEntity
    {
        public string Name { get; set; }
    }
}

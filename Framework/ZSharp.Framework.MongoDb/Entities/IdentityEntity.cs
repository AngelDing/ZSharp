
namespace ZSharp.Framework.MongoDb
{
    [CollectionName("IdentityEntity")]
    public class IdentityEntity<T> : StringKeyMongoEntity
    {
        public string Key
        {
            get;
            set;
        }
        public T Value
        {
            get;
            set;
        }
    }   
}

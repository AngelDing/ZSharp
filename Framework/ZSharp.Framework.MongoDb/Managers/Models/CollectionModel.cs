
namespace ZSharp.Framework.MongoDb.Managers
{
    public class CollectionModel : BaseModel, IModel
    {
        public DatabaseModel Database { get; set; }

        public string Namespace { get; set; }

        public long TotalCount { get; set; }

        public string FullInfo
        {
            get 
            { 
                return string.Format("數據表：{0} | 數據總數：{1}", Name, TotalCount); 
            } 
        }
    }
}

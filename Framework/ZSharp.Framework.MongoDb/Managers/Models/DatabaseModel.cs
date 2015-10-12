namespace ZSharp.Framework.MongoDb.Managers
{
    public class DatabaseModel : BaseModel, IModel
    {
        public ServerModel Server { get; set; }

        public double Size { get; set; }

        public string FullInfo 
        { 
            get 
            { 
                return string.Format("數據庫：{0} | 總容量：{1}", Name, Size); 
            } 
        }
    }
}

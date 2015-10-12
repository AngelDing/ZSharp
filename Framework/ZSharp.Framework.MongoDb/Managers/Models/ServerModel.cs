
namespace ZSharp.Framework.MongoDb.Managers
{
    public class ServerModel : BaseModel, IModel
    {
        public string IP { get; set; }

        public string Port { get; set; }

        public override string Name 
        {
            get
            {
                return string.Format("{0}:{1}", IP, Port); 
            }
            set
            { 
            }
        }

        public double TotalSize { get; set; }

        public bool IsOK { get; set; }

        public string FullInfo 
        {
            get
            { 
                return string.Format("服務器：{0} | 總容量：{1} | 狀態：{2}", 
                    Name, TotalSize, IsOK ? "良好" : "無法訪問");
            } 
        }
    }
}

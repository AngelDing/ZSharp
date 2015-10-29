
namespace ZSharp.Framework.Logging
{
    public class LogModel
    {
        public LogModel()
        {
            LogLevelType = LogLevelType.All;
        }

        public string Message { get; set; }        

        public string SystemCode { get; set; }

        public LogLevelType LogLevelType { get; set; }

        public string Detail { get; set; } 

        public string Source { get; set; }

        public override string ToString()
        {
            return this.Message;
        }
    }
}


namespace ZSharp.Framework.Logging
{
    public class LogArgumentEntity
    {
        public LogArgumentEntity()
        {
            ShowDateTime = true;
            ShowLevel = true;
            ShowLogName = true;
        }

        public string LogName { get; set; }

        public LogLevelType Level { get; set; }

        public bool ShowLevel { get; set; }

        public bool ShowDateTime { get; set; }

        public bool ShowLogName { get; set; }

        public string DateTimeFormat { get; set; }

        public bool HasDateTimeFormat
        {
            get
            {
                return !string.IsNullOrEmpty(this.DateTimeFormat);
            }
        }
    } 
}
 
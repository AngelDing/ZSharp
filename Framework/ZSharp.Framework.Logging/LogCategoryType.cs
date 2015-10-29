using System.ComponentModel;

namespace ZSharp.Framework.Logging
{
    public enum LogCategoryType
    {
        [Description("AppLog")]
        AppLog = 1,

        [Description("ErrorLog")]
        ErrorLog = 2,

        [Description("PerfLog")]
        PerfLog = 3 
    }
}

using System.Collections.Specialized;
using ZSharp.Framework.Logging.Configuration;
using ZSharp.Framework.Extensions;

namespace ZSharp.Framework.Logging.Simple
{
    public class TraceLoggerAdapter : BaseSimpleLoggerAdapter

    {
        public bool UseTraceSource { get; set; }

        public TraceLoggerAdapter()
            : base((NameValueCollection)null)
        {
            UseTraceSource = false;
        }

        public TraceLoggerAdapter(NameValueCollection properties)
            : base(properties)
        {
            UseTraceSource = properties["useTraceSource"].ToBool(false);
        }

        public TraceLoggerAdapter(LogArgumentEntity argEntity, bool isUseTraceSource)
            : base(argEntity)
        {
            UseTraceSource = isUseTraceSource;
        }

        protected override ILogger CreateLogger(LogArgumentEntity argEntity)
        {
            ILogger log = new TraceLogger(UseTraceSource, argEntity);
            return log;
        }
    }
}

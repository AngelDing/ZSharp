using System.Collections.Generic;
using System.Collections.Specialized;
using ZSharp.Framework.Logging.Configuration;
using ZSharp.Framework.Extensions;

namespace ZSharp.Framework.Logging.Simple
{
    public class ConsoleOutLoggerAdapter : BaseSimpleLoggerAdapter
    {
        private readonly bool useColor;

        public ConsoleOutLoggerAdapter()
            : base((NameValueCollection)null)
        { 
        }

        public ConsoleOutLoggerAdapter(NameValueCollection properties)
            : base(properties)
        {
            var useColorStr = properties.Get("useColor");
            if (string.IsNullOrEmpty(useColorStr) == false)
            {
                this.useColor = useColorStr.ToBool(false);
            }
        } 

        public ConsoleOutLoggerAdapter(LogArgumentEntity argEntity)
            : base(argEntity)
        { 
        }


        public ConsoleOutLoggerAdapter(LogArgumentEntity argEntity, bool useColor)
            : this(argEntity)
        {
            this.useColor = useColor;
        }       

        protected override ILogger CreateLogger(LogArgumentEntity argEntity)
        {
            ILogger log = new ConsoleOutLogger(argEntity, this.useColor);
            return log;
        }
    }
}

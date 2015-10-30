using System;
using ZSharp.Framework.Logging;

namespace Framework.Logging.Test
{
    public class CapturingLoggerEvent
    {
        public readonly CapturingLogger Source;

        public readonly LogLevelType Level;

        public readonly object MessageObject;

        public readonly Exception Exception;

        public string RenderedMessage
        {
            get { return MessageObject.ToString(); }
        }

        public CapturingLoggerEvent(CapturingLogger source, LogLevelType level, object msg, Exception ex)
        {
            Source = source;
            Level = level;
            MessageObject = msg;
            Exception = ex;
        }
    }
}

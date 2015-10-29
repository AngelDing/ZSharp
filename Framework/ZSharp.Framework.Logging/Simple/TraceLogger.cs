using System;
using System.Diagnostics;
using System.Text;

namespace ZSharp.Framework.Logging.Simple
{
    /// <summary>
    /// Logger sending everything to the trace output stream using System.Diagnostics.Trace.
    /// </summary>
    /// <remarks>
    /// Beware not to use LoggingTraceListener in combination with this logger as 
    /// this would result in an endless loop for obvious reasons!
    /// </remarks>
    public class TraceLogger : BaseSimpleLogger
    {
        private readonly bool useTraceSource;
        private TraceSource traceSource;

        private class FormatOutputMessage
        {
            private readonly TraceLogger outer;
            private readonly LogLevelType level;
            private readonly object message;
            private readonly Exception ex; 

            public FormatOutputMessage(TraceLogger outer, LogLevelType level, object message, Exception ex)
            {
                this.outer = outer;
                this.level = level;
                this.message = message;
                this.ex = ex;
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                outer.FormatOutput(sb, level, message, ex);
                return sb.ToString();
            }
        }

        public TraceLogger(bool useTrace, LogArgumentEntity argEntity)
            : base(argEntity)
        {
            useTraceSource = useTrace;
            if (useTraceSource)
            {
                traceSource = new TraceSource(argEntity.LogName, Map2SourceLevel(argEntity.Level));
            }
        }

        protected override bool IsLevelEnabled(LogLevelType level)
        {
            if (!useTraceSource)
            {
                return base.IsLevelEnabled(level);
            }
            return traceSource.Switch.ShouldTrace(Map2TraceEventType(level));
        }

        protected override void Write(LogLevelType level, object message, Exception e)
        {
            var msg = new FormatOutputMessage(this, level, message, e);
            if (traceSource != null)
            {
                traceSource.TraceEvent(Map2TraceEventType(level), 0, "{0}", msg);
            }
            else
            {
                switch (level)
                {
                    case LogLevelType.Info:
                        System.Diagnostics.Trace.TraceInformation("{0}", msg);
                        break;
                    case LogLevelType.Warn:
                        System.Diagnostics.Trace.TraceWarning("{0}", msg);
                        break;
                    case LogLevelType.Error:
                    case LogLevelType.Fatal:
                        System.Diagnostics.Trace.TraceError("{0}", msg);
                        break;
                    default:
                        System.Diagnostics.Trace.WriteLine(msg);
                        break;
                }
            }
        }

        private TraceEventType Map2TraceEventType(LogLevelType logLevel)
        {
            switch (logLevel)
            {
                case LogLevelType.Trace:
                    return TraceEventType.Verbose;
                case LogLevelType.Debug:
                    return TraceEventType.Verbose;
                case LogLevelType.Info:
                    return TraceEventType.Information;
                case LogLevelType.Warn:
                    return TraceEventType.Warning;
                case LogLevelType.Error:
                    return TraceEventType.Error;
                case LogLevelType.Fatal:
                    return TraceEventType.Critical;
                default:
                    return 0;
            }
        }

        private SourceLevels Map2SourceLevel(LogLevelType logLevel)
        {
            switch (logLevel)
            {
                case LogLevelType.All:
                case LogLevelType.Trace:
                    return SourceLevels.All;
                case LogLevelType.Debug:
                    return SourceLevels.Verbose;
                case LogLevelType.Info:
                    return SourceLevels.Information;
                case LogLevelType.Warn:
                    return SourceLevels.Warning;
                case LogLevelType.Error:
                    return SourceLevels.Error;
                case LogLevelType.Fatal:
                    return SourceLevels.Critical;
                default:
                    return SourceLevels.Off;
            }
        }
    }
}

using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text;
using ZSharp.Framework.Extensions;

namespace ZSharp.Framework.Logging.Simple
{
    public class LoggingTraceListener : TraceListener
    {
        private TraceEventType defaultTraceEventType = TraceEventType.Verbose;
        private string loggerNameFormat = "{listenerName}.{sourceName}";

        public TraceEventType DefaultTraceEventType
        {
            get { return defaultTraceEventType; }
            set { defaultTraceEventType = value; }
        }

        public string LoggerNameFormat
        {
            get { return loggerNameFormat; }
            set { loggerNameFormat = value; }
        }

        public LoggingTraceListener()
            : this(string.Empty)
        {
        }


        public LoggingTraceListener(string initializeData)
            : this(GetPropertiesFromInitString(initializeData))
        {
        }

        public LoggingTraceListener(NameValueCollection properties)
            : base()
        {
            if (properties == null)
            {
                properties = new NameValueCollection();
            }
            ApplyProperties(properties);
        }

        private void ApplyProperties(NameValueCollection props)
        {
            if (props["defaultTraceEventType"] != null)
            {
                this.defaultTraceEventType = props["defaultTraceEventType"].ToEnum(TraceEventType.Verbose);
            }
            else
            {
                this.defaultTraceEventType = TraceEventType.Verbose;
            }

            if (props["name"] != null)
            {
                this.Name = props["name"];
            }
            else
            {
                this.Name = "Diagnostics";
            }

            if (props["loggerNameFormat"] != null)
            {
                this.LoggerNameFormat = props["loggerNameFormat"];
            }
            else
            {
                this.LoggerNameFormat = "{listenerName}.{sourceName}";
            }
        }


        protected virtual void Log(TraceEventType eventType, string source,
            int id, string format, params object[] args)
        {
            source = this.LoggerNameFormat.Replace(
                "{listenerName}", this.Name).Replace("{sourceName}", "" + source);
            var log = LogManager.GetLogger(source);
            LogLevelType logLevel = MapLogLevel(eventType);

            switch (logLevel)
            {
                case LogLevelType.Trace:
                    log.Trace(format, args);
                    break;
                case LogLevelType.Debug:
                    log.Debug(format, args);
                    break;
                case LogLevelType.Info:
                    log.Info(format, args);
                    break;
                case LogLevelType.Warn:
                    log.Warn(format, args);
                    break;
                case LogLevelType.Error:
                    log.Error(format, args);
                    break;
                case LogLevelType.Fatal:
                    log.Fatal(format, args);
                    break;
                case LogLevelType.Off:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        "eventType", eventType, "invalid TraceEventType value");
            }
        }

        private static NameValueCollection GetPropertiesFromInitString(string initializeData)
        {
            NameValueCollection props = new NameValueCollection();

            if (initializeData == null)
            {
                return props;
            }

            string[] parts = initializeData.Split(';');
            foreach (string s in parts)
            {
                string part = s.Trim();
                if (part.Length == 0)
                    continue;

                int ixEquals = part.IndexOf('=');
                if (ixEquals > -1)
                {
                    string name = part.Substring(0, ixEquals).Trim();
                    string value = (ixEquals < part.Length - 1) ? part.Substring(ixEquals + 1) : string.Empty;
                    props[name] = value.Trim();
                }
                else
                {
                    props[part.Trim()] = null;
                }
            }
            return props;
        }

        public override void Write(object o)
        {
            var shouldTrace = this.Filter.ShouldTrace(
                null, this.Name, this.DefaultTraceEventType, 0, null, null, o, null);
            if ((this.Filter == null) || shouldTrace)
            {
                Log(this.DefaultTraceEventType, null, 0, "{0}", o);
            }
        }

        public override void Write(object o, string category)
        {
            if (((this.Filter == null) || this.Filter.ShouldTrace(null, this.Name, this.DefaultTraceEventType, 0, null, null, o, null)))
            {
                Log(this.DefaultTraceEventType, category, 0, "{0}", o);
            }
        }

        public override void Write(string message)
        {
            if (((this.Filter == null) || this.Filter.ShouldTrace(null, this.Name, this.DefaultTraceEventType, 0, null, null, null, null)))
            {
                Log(this.DefaultTraceEventType, null, 0, message);
            }
        }

        public override void Write(string message, string category)
        {
            if (((this.Filter == null) || this.Filter.ShouldTrace(null, this.Name, this.DefaultTraceEventType, 0, null, null, null, null)))
            {
                Log(this.DefaultTraceEventType, category, 0, message);
            }
        }

        public override void WriteLine(object o)
        {
            if (((this.Filter == null) || this.Filter.ShouldTrace(null, this.Name, this.DefaultTraceEventType, 0, null, null, o, null)))
            {
                Log(this.DefaultTraceEventType, null, 0, "{0}", o);
            }
        }

        public override void WriteLine(object o, string category)
        {
            if (((this.Filter == null) || this.Filter.ShouldTrace(null, this.Name, this.DefaultTraceEventType, 0, null, null, o, null)))
            {
                Log(this.DefaultTraceEventType, category, 0, "{0}", o);
            }
        }

        public override void WriteLine(string message)
        {
            if (((this.Filter == null) || this.Filter.ShouldTrace(null, this.Name, this.DefaultTraceEventType, 0, null, null, null, null)))
            {
                Log(this.DefaultTraceEventType, null, 0, message);
            }
        }

        public override void WriteLine(string message, string category)
        {
            if (((this.Filter == null) || this.Filter.ShouldTrace(null, this.Name, this.DefaultTraceEventType, 0, null, null, null, null)))
            {
                Log(this.DefaultTraceEventType, category, 0, message);
            }
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            if ((this.Filter == null) || this.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, null, null))
            {
                Log(eventType, source, id, "Event Id {0}", id);
            }
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            if ((this.Filter == null) || this.Filter.ShouldTrace(eventCache, source, eventType, id, message, null, null, null))
            {
                Log(eventType, source, id, message);
            }
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message, params object[] args)
        {
            if ((this.Filter == null) || this.Filter.ShouldTrace(eventCache, source, eventType, id, message, args, null, null))
            {
                Log(eventType, source, id, message, args);
            } 
        } 

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            if ((this.Filter == null) || this.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, null, data))
            {
                string fmt = GetFormat((object[])data);
                Log(eventType, source, id, fmt, data);
            }
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if ((this.Filter == null) || this.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data, null))
            {
                string fmt = GetFormat((object)data);
                Log(eventType, source, id, fmt, data);
            }
        }

        private string GetFormat(params object[] data)
        {
            if (data == null || data.Length == 0)
                return null;
            var fmt = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                fmt.Append('{').Append(i).Append('}');
                if (i < data.Length - 1)
                    fmt.Append(',');
            }
            return fmt.ToString();
        }

        private LogLevelType MapLogLevel(TraceEventType eventType)
        {
            switch (eventType)
            {
                case TraceEventType.Start:
                case TraceEventType.Stop:
                case TraceEventType.Suspend:
                case TraceEventType.Resume:
                case TraceEventType.Transfer:
                    return LogLevelType.Trace;
                case TraceEventType.Verbose:
                    return LogLevelType.Debug;
                case TraceEventType.Information:
                    return LogLevelType.Info;
                case TraceEventType.Warning:
                    return LogLevelType.Warn;
                case TraceEventType.Error:
                    return LogLevelType.Error;
                case TraceEventType.Critical:
                    return LogLevelType.Fatal;
                default:
                    return LogLevelType.Trace;
            }
        }
    }
}

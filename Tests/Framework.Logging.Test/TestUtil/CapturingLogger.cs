using System;
using System.Collections.Generic;
using ZSharp.Framework.Logging;
using ZSharp.Framework.Logging.Simple;
using ZSharp.Framework.Utils;

namespace Framework.Logging.Test
{
    public class CapturingLogger : BaseSimpleLogger
    {
        /// <summary>
        /// The adapter that created this logger instance.
        /// </summary>
        public readonly CapturingLoggerAdapter Owner;

        private volatile CapturingLoggerEvent lastEvent;

        public CapturingLoggerEvent LastEvent
        {
            get { return lastEvent; }
        }

        public void Clear()
        {
            lock (LoggerEvents)
            {
                lastEvent = null;
                LoggerEvents.Clear();
            }
        }

        public readonly IList<CapturingLoggerEvent> LoggerEvents = new List<CapturingLoggerEvent>();

        public virtual void AddEvent(CapturingLoggerEvent loggerEvent)
        {
            lastEvent = loggerEvent;
            lock (LoggerEvents)
            {
                LoggerEvents.Add(loggerEvent);
            }
            Owner.AddEvent(LastEvent);
        }

        public CapturingLogger(CapturingLoggerAdapter owner, string logName)
            : base(InitLogArgumentEntity(logName))
        {
            GuardHelper.ArgumentNotNull(() => owner);
            Owner = owner;
        }

        private static LogArgumentEntity InitLogArgumentEntity(string logName)
        {
            var argEntity = new LogArgumentEntity
            {
                LogName = logName,
                Level = LogLevelType.All,
                ShowDateTime = true,
                ShowLevel = true,
                ShowLogName = true,
                DateTimeFormat = null
            };
            return argEntity;
        }


        protected override void Write(LogLevelType level, object message, Exception exception)
        {
            CapturingLoggerEvent ev = new CapturingLoggerEvent(this, level, message, exception);
            AddEvent(ev);
        }
    }
}

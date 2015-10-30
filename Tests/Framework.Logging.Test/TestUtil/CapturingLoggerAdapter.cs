using System;
using System.Collections;
using System.Collections.Generic;
using ZSharp.Framework.Logging;

namespace Framework.Logging.Test
{
    /// <summary>
    /// An adapter, who's loggers capture all log events and send them to "AddEvent". 
    /// Retrieve the list of log events from "LoggerEvents".
    /// This logger factory is mainly for debugging and test purposes.
    /// </summary>
    public class CapturingLoggerAdapter : ILoggerAdapter
    {
        private readonly Dictionary<string, ILogger> cachedLoggers = 
            new Dictionary<string, ILogger>(StringComparer.OrdinalIgnoreCase);

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
        }

        public ILogger GetLogger(Type type)
        {
            return GetLogger(type.FullName);
        }

        public ILogger GetLogger(string name)
        {
            ILogger logger;
            if (!cachedLoggers.TryGetValue(name, out logger))
            {
                lock (((ICollection)cachedLoggers).SyncRoot)
                {
                    if (!cachedLoggers.TryGetValue(name, out logger))
                    {
                        logger = new CapturingLogger(this, name);
                        cachedLoggers[name] = logger;
                    }
                }
            }
            return logger;
        }
    }
}

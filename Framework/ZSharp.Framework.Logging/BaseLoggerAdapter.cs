using System;
using System.Collections.Concurrent;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.Logging
{
    public abstract class BaseLoggerAdapter : ILoggerAdapter
    {
        private readonly ConcurrentDictionary<string, ILogger> cacheLoggers;

        protected BaseLoggerAdapter()
        {
            cacheLoggers = new ConcurrentDictionary<string, ILogger>();
        }

        #region Implementation of ILoggerAdapter 

        /// <summary>
        /// 由指定类型获取<see cref="ILog"/>日志实例
        /// </summary>
        /// <param name="type">指定类型</param>
        /// <returns></returns>
        public ILogger GetLogger(Type type)
        {
            GuardHelper.ArgumentNotNull(() => type);
            return GetLoggerInternal(type.FullName);
        }

        public ILogger GetLogger(string name)
        {
            GuardHelper.ArgumentNotEmpty(() => name);
            return GetLoggerInternal(name);
        }

        #endregion

        /// <returns></returns>
        protected abstract ILogger CreateLogger(string name);

        protected virtual void ClearLoggerCache()
        {
            cacheLoggers.Clear();
        }

        private ILogger GetLoggerInternal(string name)
        {
            ILogger log;
            if (cacheLoggers.TryGetValue(name, out log))
            {
                return log;
            }
            log = CreateLogger(name);
            if (log == null)
            {
                var msg = string.Format("创建名称为“{0}”的日志实例时“{1}”返回空实例。", name, GetType().FullName);
                throw new NotSupportedException(msg);
            }
            cacheLoggers[name] = log;
            return log;
        }
    }
}

using System;

namespace ZSharp.Framework.Logging
{
    public abstract class BaseLogger : ILogger
    {
        public abstract bool IsTraceEnabled { get; }

        public abstract bool IsDebugEnabled { get; }

        public abstract bool IsInfoEnabled { get; }

        public abstract bool IsWarnEnabled { get; }

        public abstract bool IsErrorEnabled { get; }

        public abstract bool IsFatalEnabled { get; }

        #region ILogger Methods

        public void Trace<T>(T message)
        {
            if (IsTraceEnabled)
            {
                Write(LogLevelType.Trace, message, null);
            }
        }

        public void Trace(string format, params object[] args)
        {
            if (IsTraceEnabled)
            {
                Write(LogLevelType.Trace, FormatMessage(format, args), null);
            } 
        }

        public void Debug<T>(T message) 
        {
            if (IsDebugEnabled)
            {
                Write(LogLevelType.Debug, message, null);
            }
        }

        public void Debug(string format, params object[] args)
        {
            if (IsDebugEnabled)
            {
                Write(LogLevelType.Debug, FormatMessage(format, args), null);
            }
        }

        public void Info<T>(T message)
        {
            if (IsInfoEnabled)
            {
                Write(LogLevelType.Info, message, null);
            }
        }

        public void Info(string format, params object[] args)
        {
            if (IsInfoEnabled)
            {
                Write(LogLevelType.Info, FormatMessage(format, args), null);
            }
        }

        public void Warn<T>(T message)
        {
            if (IsWarnEnabled)
            {
                Write(LogLevelType.Warn, message, null);
            }
        }

        public void Warn(string format, params object[] args)
        {
            if (IsWarnEnabled)
            {
                Write(LogLevelType.Warn, FormatMessage(format, args), null);
            }
        }

        public void Error<T>(T message)
        {
            if (IsErrorEnabled)
            {
                Write(LogLevelType.Error, message, null);
            }
        }

        public void Error(string format, params object[] args)
        {
            if (IsErrorEnabled)
            {
                Write(LogLevelType.Error, FormatMessage(format, args), null);
            }
        }

        public void Error<T>(T message, Exception exception)
        {
            if (IsErrorEnabled)
            {
                Write(LogLevelType.Error, message, exception);
            }
        }

        public void Error(string format, Exception exception, params object[] args)
        {
            if (IsErrorEnabled)
            {
                Write(LogLevelType.Error, FormatMessage(format, args), exception);
            }
        }

        public void Fatal<T>(T message)
        {
            if (IsFatalEnabled)
            {
                Write(LogLevelType.Fatal, message, null);
            }
        }

        public void Fatal(string format, params object[] args)
        {
            if (IsFatalEnabled)
            {
                Write(LogLevelType.Fatal, FormatMessage(format, args), null);
            }
        }

        public void Fatal<T>(T message, Exception exception)
        {
            if (IsFatalEnabled)
            {
                Write(LogLevelType.Fatal, message, exception);
            }
        }

        public void Fatal(string format, Exception exception, params object[] args)
        {
            if (IsFatalEnabled)
            {
                Write(LogLevelType.Fatal, FormatMessage(format, args), exception);
            }
        }

        private string FormatMessage(string format, params object[] args)
        {
            var msg = format;
            if (args.Length > 0)
            {
                msg = string.Format(format, args);
            }
            return msg;
        }

        #endregion

        protected abstract void Write(LogLevelType level, object message, Exception exception);
    }
}

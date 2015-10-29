using System;

namespace ZSharp.Framework.Logging.Simple
{
    public sealed class NoOpLogger : ILogger
    {
        public bool IsTraceEnabled { get { return false; } }

        public bool IsDebugEnabled { get { return false; } }

        public bool IsInfoEnabled { get { return false; } }

        public bool IsWarnEnabled { get { return false; } }

        public bool IsErrorEnabled { get { return false; } }

        public bool IsFatalEnabled { get { return false; } }

        public void Trace<T>(T message)
        {
            // NOP - no operation
        }

        public void Trace(string format, params object[] args)
        {
            // NOP - no operation
        }

        public void Debug<T>(T message)
        { 
        }

        public void Debug(string format, params object[] args)
        {
            // NOP - no operation
        }

        public void Info<T>(T message)
        {
        }

        public void Info(string format, params object[] args)
        {
            // NOP - no operation
        }

        public void Warn<T>(T message)
        {
        }

        public void Warn(string format, params object[] args)
        {
            // NOP - no operation
        }

        public void Error<T>(T message)
        {
        }

        public void Error(string format, params object[] args)
        {
            // NOP - no operation
        }

        public void Error<T>(T message, Exception exception)
        {
            // NOP - no operation
        }

        public void Error(string format, Exception exception, params object[] args)
        {
            // NOP - no operation
        }

        public void Fatal<T>(T message)
        {
            // NOP - no operation
        }

        public void Fatal(string format, params object[] args)
        {
            // NOP - no operation
        }

        public void Fatal<T>(T message, Exception exception)
        {
            // NOP - no operation
        }

        public void Fatal(string format, Exception exception, params object[] args)
        {
            // NOP - no operation
        }
    }
}

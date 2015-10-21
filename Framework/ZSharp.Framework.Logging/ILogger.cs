using System;

namespace ZSharp.Framework.Logging
{
    public interface ILogger
    {
        bool IsTraceEnabled { get; }

        bool IsDebugEnabled { get; } 

        bool IsErrorEnabled { get; }

        bool IsFatalEnabled { get; }

        bool IsInfoEnabled { get; }
         
        bool IsWarnEnabled { get; }
         
        //void Trace<T>(T message);

        void Trace(string format, params object[] args);

        //void Debug<T>(T message);

        void Debug(string format, params object[] args);

        //void Info<T>(T message);

        void Info(string format, params object[] args);

        //void Warn<T>(T message);

        void Warn(string format, params object[] args);

        //void Error<T>(T message);

        void Error(string format, params object[] args);

        //void Error<T>(T message, Exception exception);

        void Error(string format, Exception exception, params object[] args);

        //void Fatal<T>(T message);

        void Fatal(string format, params object[] args);

        //void Fatal<T>(T message, Exception exception);

        void Fatal(string format, Exception exception, params object[] args);

    }
}

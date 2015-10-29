using System;

namespace ZSharp.Framework.Logging.Simple
{
    public sealed class NoOpLoggerAdapter : ILoggerAdapter
    {
        private static readonly ILogger nopLogger = new NoOpLogger();

        public ILogger GetLogger(Type type)
        {
            return nopLogger;
        }

        public ILogger GetLogger(string key)
        {
            return nopLogger;
        }
    } 
}

using System;
using ZSharp.Framework.Logging;

namespace Framework.Logging.Test
{
    public class MissingCtorLoggerAdapter : ILoggerAdapter
    {
        private string foo;

        public MissingCtorLoggerAdapter(string foo)
        {
            this.foo = foo;
        }

        public ILogger GetLogger(Type type)
        {
            throw new NotImplementedException();
        }

        public ILogger GetLogger(string name)
        {
            throw new NotImplementedException();
        }
    }
}
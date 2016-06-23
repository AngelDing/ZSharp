using System;

namespace ZSharp.Framework.Exceptions
{
    public class FrameworkException : Exception
    {
        public FrameworkException() : base() { }

        public FrameworkException(string message) : base(message) { }

        public FrameworkException(string message, Exception innerException) : base(message, innerException) { }

        public FrameworkException(string format, params object[] args) : base(string.Format(format, args)) { }
    }
}
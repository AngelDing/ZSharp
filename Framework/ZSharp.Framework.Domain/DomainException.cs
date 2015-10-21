using System;

namespace ZSharp.Framework.Domain
{
    public class DomainException : FrameworkException
    {
        public DomainException() : base() { }

        public DomainException(string message) : base(message) { }

        public DomainException(string message, Exception innerException) : base(message, innerException) { }

        public DomainException(string format, params object[] args) : base(string.Format(format, args)) { }
    }
}

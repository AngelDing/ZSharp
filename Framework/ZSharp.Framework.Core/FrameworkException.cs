using System;

namespace ZSharp.Framework
{
    public class FrameworkException : Exception
    {
        public FrameworkException(string message)
            : base(message)
        {
        }
    }
}
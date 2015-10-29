using System;

namespace ZSharp.Framework.Logging.Configuration
{
    public class ConfigurationException : ApplicationException
    {
        public ConfigurationException()
        {
        }

        public ConfigurationException(string message)
            : base(message)
        {
        }

        public ConfigurationException(string message, Exception rootCause)
            : base(message, rootCause)
        {
        } 
    }
}

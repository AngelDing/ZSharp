using System;
using System.Collections.Specialized;
using ZSharp.Framework.Logging.Configuration;
using ZSharp.Framework.Extensions; 

namespace ZSharp.Framework.Logging.Simple
{
    public abstract class BaseSimpleLoggerAdapter : BaseLoggerAdapter
    {
        public LogArgumentEntity ArgumentEntity { get; set; }

        protected BaseSimpleLoggerAdapter(NameValueCollection properties) :
            this(InitLogArgumentEntity(properties))
        {
        }

        private static LogArgumentEntity InitLogArgumentEntity(NameValueCollection properties)
        {
            var argEntity = new LogArgumentEntity();
            if (properties != null)
            {
                argEntity.Level = properties.Get("level").ToEnum(LogLevelType.All);
                argEntity.ShowDateTime = properties.Get("showDateTime").ToBool(true);
                argEntity.ShowLogName = properties.Get("showLogName").ToBool(true);
                argEntity.ShowLevel = properties.Get("showLevel").ToBool(true);
                argEntity.DateTimeFormat = properties.Get("dateTimeFormat");
            }
            return argEntity;
        }

        protected BaseSimpleLoggerAdapter(LogArgumentEntity argEntity)
            : base()
        {
            ArgumentEntity = argEntity;
        }

        protected override ILogger CreateLogger(string name)
        {
            ArgumentEntity.LogName = name;
            return CreateLogger(ArgumentEntity);
        }

        protected abstract ILogger CreateLogger(LogArgumentEntity argEntity);
    }
}

using System;
using System.Globalization;
using System.Text;
 
namespace ZSharp.Framework.Logging.Simple
{
    public abstract class BaseSimpleLogger : BaseLogger
    {
        private readonly LogArgumentEntity argumentEntity;

        public LogArgumentEntity ArgumentEntity
        {
            get { return argumentEntity; }
        }        

        public BaseSimpleLogger(LogArgumentEntity argEntity)
        {
            argumentEntity = argEntity;
        }

        protected virtual void FormatOutput(StringBuilder stringBuilder, LogLevelType level, object message, Exception e)
        {
            if (stringBuilder == null)
            {
                throw new ArgumentNullException("stringBuilder");
            }
            if (ArgumentEntity.ShowDateTime)
            {
                if (ArgumentEntity.HasDateTimeFormat)
                {
                    stringBuilder.Append(DateTimeOffset.Now.ToString(ArgumentEntity.DateTimeFormat, CultureInfo.InvariantCulture));
                }
                else
                {
                    stringBuilder.Append(DateTimeOffset.Now);
                }

                stringBuilder.Append(" ");
            }

            if (ArgumentEntity.ShowLevel)
            {
                stringBuilder.Append(("[" + level.ToString().ToUpper() + "]").PadRight(8));
            }

            if (ArgumentEntity.ShowLogName)
            {
                stringBuilder.Append(ArgumentEntity.LogName).Append(" - ");
            }

            stringBuilder.Append(message);

            if (e != null)
            {
                stringBuilder.Append(Environment.NewLine).Append(ExceptionFormatter.Format(e));
            }
        }

        protected virtual bool IsLevelEnabled(LogLevelType level)
        {
            int iLevel = (int)level;
            int iCurrentLogLevel = (int)ArgumentEntity.Level;
            return (iLevel >= iCurrentLogLevel);
        }

        #region ILogger Members

        public override bool IsTraceEnabled
        {
            get { return IsLevelEnabled(LogLevelType.Trace); }
        }

        public override bool IsDebugEnabled
        {
            get { return IsLevelEnabled(LogLevelType.Debug); }
        }

        public override bool IsInfoEnabled
        {
            get { return IsLevelEnabled(LogLevelType.Info); }
        }

        public override bool IsWarnEnabled
        {
            get { return IsLevelEnabled(LogLevelType.Warn); }
        }

        public override bool IsErrorEnabled
        {
            get { return IsLevelEnabled(LogLevelType.Error); }
        }

        public override bool IsFatalEnabled
        {
            get { return IsLevelEnabled(LogLevelType.Fatal); }
        }

        #endregion
    }
}

using Xunit;
using FluentAssertions;
using ZSharp.Framework.Logging.Simple;
using ZSharp.Framework.Logging;
using System;
using System.Collections.Specialized;

namespace Framework.Logging.Test
{
    public class BaseSimpleLoggerTests
    {
        private class ConcreteLogger : BaseSimpleLogger
        {
            public ConcreteLogger(LogArgumentEntity argEntiy)
                : base(argEntiy)
            { }

            protected override void Write(LogLevelType level, object message, Exception exception)
            {
                throw new NotImplementedException();
            }
        }

        private class ConcreteLoggerFactory : BaseSimpleLoggerAdapter
        {
            public ConcreteLoggerFactory(NameValueCollection properties)
                : base(properties)
            {
            }

            protected override ILogger CreateLogger(LogArgumentEntity argEntity)
            {
                return new ConcreteLogger(argEntity);
            }
        }

        [Fact]
        public void logging_base_simple_logger_default_values_test()
        {
            var logger = (BaseSimpleLogger)new ConcreteLoggerFactory(null).GetLogger("x");
            logger.ArgumentEntity.LogName.Should().Be("x");
            logger.ArgumentEntity.ShowLogName.Should().BeTrue();
            logger.ArgumentEntity.ShowDateTime.Should().BeTrue();
            logger.ArgumentEntity.ShowLevel.Should().BeTrue();
            logger.ArgumentEntity.HasDateTimeFormat.Should().BeFalse();
            logger.ArgumentEntity.DateTimeFormat.Should().BeNullOrEmpty();
            logger.ArgumentEntity.Level.Should().Be(LogLevelType.All);
        }

        [Fact]
        public void logging_base_simple_logger_configured_values_test()
        {
            var props = new NameValueCollection();
            props["showLogName"] = "false";
            props["showLevel"] = "false";
            props["showDateTime"] = "false";
            props["dateTimeFormat"] = "MM";
            props["level"] = "Info";

            var logger = (BaseSimpleLogger)new ConcreteLoggerFactory(props).GetLogger("x");
            logger.ArgumentEntity.LogName.Should().Be("x");
            logger.ArgumentEntity.ShowLogName.Should().BeFalse();
            logger.ArgumentEntity.ShowDateTime.Should().BeFalse();
            logger.ArgumentEntity.ShowLevel.Should().BeFalse();
            logger.ArgumentEntity.HasDateTimeFormat.Should().BeTrue();
            logger.ArgumentEntity.DateTimeFormat.Should().Be("MM");
            logger.ArgumentEntity.Level.Should().Be(LogLevelType.Info);
        }
    }
}

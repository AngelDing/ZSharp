using Moq;
using Xunit;
using System;
using FluentAssertions;
using System.Diagnostics;
using ZSharp.Framework.Logging;
using ZSharp.Framework.Logging.Simple;
using ZSharp.Framework.Logging.Configuration;

namespace Framework.Logging.Test
{
    public class LogManagerTests
    {
        public LogManagerTests()
        {
            LogManager.Reset();         
        }

        [Fact]
        public void logging_adapter_property_test()
        {
            ILoggerAdapter adapter = new NoOpLoggerAdapter();
            LogManager.Adapter = adapter;

            LogManager.Adapter.Should().Be(adapter);
            Assert.Throws<ArgumentNullException>(delegate { LogManager.Adapter = null; });
        }

        [Fact]
        public void logging_manager_reset_test()
        {
            LogManager.Reset();
            LogManager.ConfigurationReader.Should().BeOfType<DefaultConfigurationReader>();
            Assert.Throws<ArgumentNullException>(delegate { LogManager.Reset(null); });

            var mockConfigReader = new Mock<IConfigurationReader>();
            mockConfigReader
                .Setup(b => b.GetSection(LogManager.LOGGING_SECTION))
                .Returns(new TraceLoggerAdapter());
            LogManager.Reset(mockConfigReader.Object);
            LogManager.Adapter.Should().BeOfType<TraceLoggerAdapter>();
        }

        [Fact]
        public void logging_manager_configure_from_configuration_reader_test()
        {
            var mockConfigReader = new Mock<IConfigurationReader>();

            mockConfigReader
                .Setup(b => b.GetSection(LogManager.LOGGING_SECTION))
                .Returns(null);
            LogManager.Reset(mockConfigReader.Object);
            var logger = LogManager.GetLogger<LogManagerTests>();
            logger.Should().BeOfType<NoOpLogger>();

            mockConfigReader
                .Setup(b => b.GetSection(LogManager.LOGGING_SECTION))
                .Returns(new TraceLoggerAdapter());
            LogManager.Reset(mockConfigReader.Object);
            logger = LogManager.GetLogger(typeof(LogManagerTests));
            logger.Should().BeOfType<TraceLogger>();

            mockConfigReader
               .Setup(b => b.GetSection(LogManager.LOGGING_SECTION))
               .Returns(new LogSetting(typeof(ConsoleOutLoggerAdapter), null));
            LogManager.Reset(mockConfigReader.Object);
            logger = LogManager.GetLogger(typeof(LogManagerTests));
            logger.Should().BeOfType<ConsoleOutLogger>();

            mockConfigReader
              .Setup(b => b.GetSection(LogManager.LOGGING_SECTION))
              .Returns(new object());
            LogManager.Reset(mockConfigReader.Object);
            var exception = Assert.Throws<ConfigurationException>(delegate
            {
                logger = LogManager.GetLogger(typeof(LogManagerTests));
            });
            var exceptMsg = string.Format(
               "ConfigurationReader {0} returned unknown settings instance of type System.Object",
                mockConfigReader.Object.GetType().FullName);
            exception.Message.Should().Be(exceptMsg);
        }

        [Fact]
        public void logging_configure_from_standalone_config_test()
        {
            const string xml =
                @"<?xml version='1.0' encoding='UTF-8' ?>
                <logging>
                  <loggerAdapter type='ZSharp.Framework.Logging.Simple.ConsoleOutLoggerAdapter, ZSharp.Framework.Logging'>
                  </loggerAdapter>
                </logging>";
            var logger = GetLog(xml);
            logger.Should().BeOfType<ConsoleOutLogger>();
        }


        [Fact]
        public void logging_invalid_adapter_type_test()
        {
            const string xml =
                @"<?xml version='1.0' encoding='UTF-8' ?>
                <logging>
                  <loggerAdapter type='ZSharp.Framework.Logging.Simple.NonExistentAdapter, ZSharp.Framework.Logging'>
                  </loggerAdapter>
                </logging>";
            Assert.Throws<ConfigurationException>(delegate { GetLog(xml); });
        }

        [Fact]
        public void logging_adapter_not_implement_interface_test()
        {
            const string xml =
                @"<?xml version='1.0' encoding='UTF-8' ?>
                <logging>
                  <loggerAdapter type='Framework.Logging.Test.StandaloneConfigurationReader, Framework.Logging.Test'>
                  </loggerAdapter>
                </logging>";
            Assert.Throws<NotImplementedException>(delegate { GetLog(xml); });
        }

        [Fact]
        public void logging_adapter_not_have_correct_ctors_test()
        {
            const string xml =
                @"<?xml version='1.0' encoding='UTF-8' ?>
                <logging>
                  <loggerAdapter type='Framework.Logging.Test.MissingCtorLoggerAdapter, Framework.Logging.Test'>
                  </loggerAdapter>
                </logging>";
            Assert.Throws<ConfigurationException>(delegate { GetLog(xml); });
        }

        [Fact]
        public void logging_adapter_not_have_correct_ctors_with_args_test()
        {
            const string xml =
                @"<?xml version='1.0' encoding='UTF-8' ?>
                <logging>
                    <loggerAdapter type='Framework.Logging.Test.MissingCtorLoggerAdapter, Framework.Logging.Test'>
                        <arg key='level' value='DEBUG' />
                    </loggerAdapter>
                </logging>";
            Assert.Throws<ConfigurationException>(delegate { GetLog(xml); });
        }

        [Fact]
        public void logging_invalid_xml_section_test()
        {
            const string xml =
                @"<?xml version='1.0' encoding='UTF-8' ?>
                <foo>
                    <logging>
                      <loggerAdapter type='Framework.Logging.Test.MissingCtorLoggerAdapter, Framework.Logging.Test'>
                            <arg key='level' value='DEBUG' />
                      </loggerAdapter>
                    </logging>
                </foo>";
            var logger = GetLog(xml);
            var noOpLogger = logger as NoOpLogger;
            noOpLogger.Should().NotBeNull();
        }

        private static ILogger GetLog(string xml)
        {
            var configReader = new StandaloneConfigurationReader(xml);
            LogManager.Reset(configReader);
            return LogManager.GetLogger(typeof(LogManagerTests));
        }
    }
}
using Xunit;
using FluentAssertions;
using ZSharp.Framework.Logging.Configuration;
using ZSharp.Framework.Logging.Simple;

namespace Framework.Logging.Test
{
    public class ConfigurationSectionHandlerTests
    {
        [Fact]
        public void logging_no_parent_sections_allowed_test()
        {
            var handler = new ConfigurationSectionHandler();
            var exception = Assert.Throws<ConfigurationException>(delegate
            {
                handler.Create(new LogSetting(typeof(ConsoleOutLoggerAdapter), null), null, null);
            });
            exception.Message.Should().Be("parent configuration sections are not allowed");
        }

        [Fact]
        public void logging_too_many_adapter_elements_test()
        {
            const string xml =
            @"<?xml version='1.0' encoding='UTF-8' ?>
            <logging>
              <loggerAdapter type='ZSharp.Framework.Logging.Simple.ConsoleOutLoggerAdapter, ZSharp.Framework.Logging'>
              </loggerAdapter>
              <loggerAdapter type='ZSharp.Framework.Logging.Simple.ConsoleOutLoggerAdapter, ZSharp.Framework.Logging'>
              </loggerAdapter>
            </logging>";
            var reader = new StandaloneConfigurationReader(xml);
            var exception = Assert.Throws<ConfigurationException>(delegate
            {
                reader.GetSection(null);
            });
            exception.Message.Should().Be("Only one <loggerAdapter> element allowed");
        }

        [Fact]
        public void logging_no_type_element_for_adapter_declaration_test()
        {
            const string xml =
            @"<?xml version='1.0' encoding='UTF-8' ?>
            <logging>
              <loggerAdapter clazz='ZSharp.Framework.Logging.Simple.ConsoleOutLoggerAdapter, ZSharp.Framework.Logging'>
                <arg key='level' value='DEBUG' />
              </loggerAdapter>
            </logging>";
            var reader = new StandaloneConfigurationReader(xml);
            var exception = Assert.Throws<ConfigurationException>(delegate
            {
                reader.GetSection(null);
            });
            exception.Message.Should().Be("Required Attribute 'type' not found in element 'loggerAdapter'");
        }

        [Fact]
        public void logging_no_key_element_for_adapter_arguments_test()
        {
            const string xml =
            @"<?xml version='1.0' encoding='UTF-8' ?>
            <logging>
              <loggerAdapter type='ZSharp.Framework.Logging.Simple.ConsoleOutLoggerAdapter, ZSharp.Framework.Logging'>
                <arg kez='level' value='DEBUG' />
              </loggerAdapter>
            </logging>";
            var reader = new StandaloneConfigurationReader(xml);
            var exception = Assert.Throws<ConfigurationException>(delegate
            {
                reader.GetSection(null);
            });
            exception.Message.Should().Be("Required Attribute 'key' not found in element 'arg'");
        }

        [Fact]
        public void logging_console_short_cut_test()
        {
            const string xml =
            @"<?xml version='1.0' encoding='UTF-8' ?>
            <logging>
              <loggerAdapter type='CONSOLE'/>
            </logging>";
            var reader = new StandaloneConfigurationReader(xml);
            var setting = reader.GetSection(null) as LogSetting;
            setting.Should().NotBeNull();
            setting.FactoryAdapterType.Should().Be(typeof(ConsoleOutLoggerAdapter));
        }

        [Fact]
        public void logging_trace_short_cut_test()
        {
            const string xml =
            @"<?xml version='1.0' encoding='UTF-8' ?>
            <logging>
              <loggerAdapter type='TRACE'/>
            </logging>";
            var reader = new StandaloneConfigurationReader(xml);
            var setting = reader.GetSection(null) as LogSetting;
            setting.Should().NotBeNull();
            setting.FactoryAdapterType.Should().Be(typeof(TraceLoggerAdapter));
        }

        [Fact]
        public void logging_no_op_short_cut_test()
        {
            const string xml =
            @"<?xml version='1.0' encoding='UTF-8' ?>
            <logging>
              <loggerAdapter type='NOOP'/>
            </logging>";
            var reader = new StandaloneConfigurationReader(xml);
            var setting = reader.GetSection(null) as LogSetting;

            setting.Should().NotBeNull();
            setting.FactoryAdapterType.Should().Be(typeof(NoOpLoggerAdapter));
        }

        [Fact]
        public void logging_argument_keys_case_insensitive_test()
        {
            const string xml =
            @"<?xml version='1.0' encoding='UTF-8' ?>
            <logging>
              <loggerAdapter type='CONSOLE'>
                <arg key='LeVel1' value='DEBUG' />
                <arg key='LEVEL2' value='DEBUG' />
                <arg key='level3' value='DEBUG' />
              </loggerAdapter>
            </logging>";
            var reader = new StandaloneConfigurationReader(xml);
            var setting = reader.GetSection(null) as LogSetting;
            setting.Should().NotBeNull();
            setting.Properties.Count.Should().Be(3);

            var expectedValue = "DEBUG";
            setting.Properties.Get("level1").Should().Be(expectedValue);
            setting.Properties.Get("level2").Should().Be(expectedValue);
            setting.Properties.Get("level3").Should().Be(expectedValue);
        }
    }
}

using Xunit;
using FluentAssertions;
using ZSharp.Framework.Logging.Simple;
using ZSharp.Framework.Logging;

namespace Framework.Logging.Test
{
    public class CapturingLoggerTests
    {
        [Fact]
        public void logging_can_change_log_level_test()
        {
            var adapter = new CapturingLoggerAdapter();
            var testLogger = (CapturingLogger)adapter.GetLogger("test");
            testLogger.ArgumentEntity.Level.Should().Be(LogLevelType.All);

            testLogger.Trace("message1");
            testLogger.LoggerEvents.Count.Should().Be(1);

            testLogger.ArgumentEntity.Level = LogLevelType.Debug;
            testLogger.Trace("message2");
            testLogger.LastEvent.MessageObject.Should().Be("message1");
        }

        [Fact]
        public void logging_clear_events_test()
        {
            var adapter = new CapturingLoggerAdapter();
            var testLogger = (CapturingLogger)adapter.GetLogger("test");
            testLogger.Trace("message1");
            testLogger.Trace("message2");
            testLogger.LastEvent.Should().NotBeNull();
            testLogger.LoggerEvents.Count.Should().Be(2);

            testLogger.Clear();
            testLogger.LastEvent.Should().BeNull();
            testLogger.LoggerEvents.Count.Should().Be(0);
        }

        [Fact]
        public void logging_adapter_clear_events_test()
        {
            var adapter = new CapturingLoggerAdapter();
            var testLogger = (CapturingLogger)adapter.GetLogger("test");
            testLogger.Trace("message1");
            testLogger.Trace("message2");
            adapter.LastEvent.Should().NotBeNull();
            adapter.LoggerEvents.Count.Should().Be(2);

            adapter.Clear();
            adapter.LastEvent.Should().BeNull();
            adapter.LoggerEvents.Count.Should().Be(0);
        }

        [Fact]
        public void logging_captures_individual_events_test()
        {
            var adapter = new CapturingLoggerAdapter();
            var testLogger = (CapturingLogger)adapter.GetLogger("test");
            testLogger.Trace("message1");
            testLogger.Trace("message2");

            testLogger.LoggerEvents.Count.Should().Be(2);
            testLogger.LastEvent.MessageObject.Should().Be("message2");
            testLogger.LoggerEvents[0].MessageObject.Should().Be("message1");
            testLogger.LoggerEvents[1].MessageObject.Should().Be("message2");

            testLogger.Clear();
            testLogger.LastEvent.Should().BeNull();
            testLogger.LoggerEvents.Count.Should().Be(0);
        }

        [Fact]
        public void logging_adapter_captures_all_events_test()
        {
            var adapter = new CapturingLoggerAdapter();
            var testLogger = (CapturingLogger)adapter.GetLogger("test");
            var test2Logger = (CapturingLogger)adapter.GetLogger("test2");
            testLogger.Trace("message1");
            test2Logger.Trace("message2");

            testLogger.LoggerEvents.Count.Should().Be(1);
            testLogger.LastEvent.MessageObject.Should().Be("message1");
            test2Logger.LoggerEvents.Count.Should().Be(1);
            test2Logger.LastEvent.MessageObject.Should().Be("message2");

            adapter.LoggerEvents.Count.Should().Be(2);
            adapter.LoggerEvents[0].MessageObject.Should().Be("message1");
            test2Logger.LoggerEvents.Count.Should().Be(1);
            adapter.LoggerEvents[1].MessageObject.Should().Be("message2");
        }
    }
}

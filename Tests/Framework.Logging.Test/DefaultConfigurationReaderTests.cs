using Xunit;
using FluentAssertions;
using ZSharp.Framework.Logging.Configuration;
using System.Collections.Specialized;

namespace Framework.Logging.Test
{
    public class DefaultConfigurationReaderTests
    {
        [Fact]
        public void logging_reads_appconfig_test()
        {
            var section = new DefaultConfigurationReader().GetSection("appSettings");
            var value = ((NameValueCollection)section)["appConfigCheck"];
            value.Should().Be("FromAppConfig");
        }
    }
}

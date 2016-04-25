using Xunit;
using ZSharp.Framework.RabbitMq;

namespace Framework.RabbitMq.Test
{
    public class RabbitHutchTest
    {
        [Fact]
        public void create_bus_test()
        {
            using (var bus = RabbitHutch.CreateBus())
            {
                Assert.NotNull(bus);
            }
        }

        [Fact]
        public void bus_publish_test()
        {
            using (var bus = RabbitHutch.CreateBus())
            {
                bus.Publish("Hello Word!");
            }
        }
    }
}

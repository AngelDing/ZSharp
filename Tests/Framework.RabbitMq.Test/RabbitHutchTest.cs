using Xunit;
using FluentAssertions;
using ZSharp.Framework.RabbitMq;
using System.Threading.Tasks;
using System;
using System.Threading;

namespace Framework.RabbitMq.Test
{
    public class RabbitHutchTest : BaseRabbitMqTest
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
                bus.Publish(GetMyTestMessage());
            }
        }

        [Fact]
        public void bus_sub_test()
        {
            Task.Factory.StartNew(
                () => PublishMessages());

            Thread.Sleep(1000);

            Task.Factory.StartNew(
                () => SubscribeMessagesA());

            Thread.Sleep(1000);

            Task.Factory.StartNew(
                () => SubscribeMessagesB());

            while (true)
            {
            }
        }

        [Fact]
        public void bus_send_test()
        {
            using (var bus = RabbitHutch.CreateBus())
            {                
                bus.Send(QueueName, GetMyTestMessage());
            }
        }

        [Fact]
        public void bus_receive_test()
        {
            using (var sendBus = RabbitHutch.CreateBus())
            {
                for (var i = 0; i < 3; i++)
                {
                    sendBus.Send(QueueName, "Hello World!");
                    sendBus.Send(QueueName, GetMyTestMessage());
                }
            }

            var receiveBus = RabbitHutch.CreateBus();
            DisposableObjs.Add(receiveBus);
            receiveBus.Receive(QueueName, x => x
                .Add<string>(message => { StringHandler(message); })
                .Add<MyTestMessage>(message => MyTestMessageHandlerAsync(message)));
            Thread.Sleep(TimeSpan.FromSeconds(10));
        }

        #region Private Methods

        private void SubscribeMessagesA()
        {
            using (var bus = RabbitHutch.CreateBus())
            {
                bus.Subscribe<MyTestMessage>("A", message => MyTestMessageHandlerAsync(message));
                while (true)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        private void SubscribeMessagesB()
        {
            using (var bus = RabbitHutch.CreateBus())
            {
                bus.Subscribe<MyTestMessage>("B", message => MyTestMessageHandlerAsync(message));
                while (true)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        private void PublishMessages()
        {
            using (var bus = RabbitHutch.CreateBus())
            {
                while (true)
                {
                    bus.Publish(GetMyTestMessage());
                    Thread.Sleep(1000);
                }
            }
        }       

        private void StringHandler(string message)
        {
            message.Should().Be("Hello World!");
        }
       
        #endregion
    }
}

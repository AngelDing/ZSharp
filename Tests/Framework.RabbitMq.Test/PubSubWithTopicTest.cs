using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ZSharp.Framework.RabbitMq;

namespace Framework.RabbitMq.Test
{
    public class PubSubWithTopicTest : BaseRabbitMqTest
    {
        [Fact]
        public void bus_pub_sub_with_topic_test()
        {
            Task.Factory.StartNew(
                () => PublishMessagesA());

            Thread.Sleep(100);

            Task.Factory.StartNew(
                () => SubscribeMessagesA());

            Thread.Sleep(100);

            Task.Factory.StartNew(
                () => SubscribeMessagesB());

            Thread.Sleep(100);

            Task.Factory.StartNew(
                () => SubscribeMessagesC());

            while (true)
            {
            }
        }

        private void SubscribeMessagesC()
        {
            using (var bus = RabbitHutch.CreateBus())
            {
                bus.Subscribe<MyTestMessage>(
                    "C",
                    message => MyTestMessageHandlerAsync(message),
                    x => x.WithTopic("#.C.#"));
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
                bus.Subscribe<MyTestMessage>(
                    "B",
                    message => MyTestMessageHandlerAsync(message),
                    x => x.WithTopic("*.B.*"));
                while (true)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        private void SubscribeMessagesA()
        {
            using (var bus = RabbitHutch.CreateBus())
            {
                bus.Subscribe<MyTestMessage>(
                    "A", 
                    message => MyTestMessageHandlerAsync(message), 
                    x => x.WithTopic("topic.A.*"));
                while (true)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        private void PublishMessagesA()
        {
            using (var bus = RabbitHutch.CreateBus())
            {
                while (true)
                {
                    bus.Publish(GetMyTestMessage("topic.A.XX"), "topic.A.XX");
                    bus.Publish(GetMyTestMessage("topic.B.YY"), "topic.B.YY");
                    bus.Publish(GetMyTestMessage("topic.B.YY.MM"), "topic.B.YY.MM"); //無訂閱者
                    bus.Publish(GetMyTestMessage("topic.C.ZZ"), "topic.C.ZZ");
                    bus.Publish(GetMyTestMessage("topic.C.MM.NN"), "topic.C.MM.NN");
                    Thread.Sleep(1000);
                }
            }
        }
    }
}

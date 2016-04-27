using ZSharp.Framework.Utils;

namespace ZSharp.Framework.RabbitMq
{
    public class MessageReceivedInfo
    {
        public string ConsumerTag { get; set; }

        public ulong DeliverTag { get; set; }

        public bool Redelivered { get; set; }

        public string Exchange { get; set; }

        public string RoutingKey { get; set; }

        public string Queue { get; set; }

        public MessageReceivedInfo() { }

        public MessageReceivedInfo(
            string consumerTag,
            ulong deliverTag,
            bool redelivered,
            string exchange,
            string routingKey,
            string queue)
        {
            ConsumerTag = consumerTag;
            DeliverTag = deliverTag;
            Redelivered = redelivered;
            Exchange = exchange;
            RoutingKey = routingKey;
            Queue = queue;
        }
    }
}

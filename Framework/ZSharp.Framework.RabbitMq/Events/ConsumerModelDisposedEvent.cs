namespace ZSharp.Framework.RabbitMq
{
    public class ConsumerModelDisposedEvent
    {
        public string ConsumerTag { get; private set; }

        public ConsumerModelDisposedEvent(string consumerTag)
        {
            ConsumerTag = consumerTag;
        }
    }
}
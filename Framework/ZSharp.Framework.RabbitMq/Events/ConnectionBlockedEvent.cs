namespace ZSharp.Framework.RabbitMq
{
    public class ConnectionBlockedEvent
    {
        public string Reason { get; private set; }

        public ConnectionBlockedEvent(string reason)
        {
            Reason = reason;
        }
    }
}
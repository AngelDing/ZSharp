namespace ZSharp.Framework.RabbitMq
{
    public class MessageReturnedInfo
    {
        private string exchange;
        private string replyText;
        private string routingKey;

        public MessageReturnedInfo(string exchange, string routingKey, string replyText)
        {
            this.exchange = exchange;
            this.routingKey = routingKey;
            this.replyText = replyText;
        }
    }
}
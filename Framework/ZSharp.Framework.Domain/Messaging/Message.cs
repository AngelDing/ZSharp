using System;

namespace ZSharp.Framework.Domain
{
    public class Message
    {
        public Message(string body, string msgType, DateTimeOffset? deliveryDate = null, string correlationId = null)
        {
            this.Body = body;
            this.DeliveryDate = deliveryDate;
            this.CorrelationId = correlationId;
            this.MessageType = msgType;
        }

        public string Body { get; private set; }

        public string CorrelationId { get; private set; }

        public DateTimeOffset? DeliveryDate { get; private set; }

        public string MessageType { get; private set; }
    }
}

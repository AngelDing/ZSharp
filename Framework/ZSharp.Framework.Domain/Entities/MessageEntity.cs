using System;
using ZSharp.Framework.SqlDb;

namespace ZSharp.Framework.Domain
{
    public class MessageEntity : EfEntity<long>
    {
        public string Body { get; set; }

        public string MessageType { get; set; }

        public DateTimeOffset DeliveryDate { get; set; }

        public string CorrelationId { get; set; }
    }
}

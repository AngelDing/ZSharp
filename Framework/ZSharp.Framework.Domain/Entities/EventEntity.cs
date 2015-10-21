using System;
using ZSharp.Framework.SqlDb;

namespace ZSharp.Framework.Domain
{
    public class EventEntity : EfEntity<long>
    {
        public Guid AggregateId { get; set; }

        public string AggregateType { get; set; }

        public int Version { get; set; }

        /// <summary>
        /// 帶版本號的事件類型，用於反序列化
        /// </summary>
        public string VersionedEventType { get; set; }

        public string Payload { get; set; }

        public string CorrelationId { get; set; }

        // TODO: Following could be very useful for when rebuilding the read model from the event store, 
        // to avoid replaying every possible event in the system
        // public string EventType { get; set; }
    }
}

using System;
using ZSharp.Framework.SqlDb;

namespace ZSharp.Framework.Domain
{
    public class DomainEventEntity : EfEntity<Guid>
    {
        public Guid AggregateId { get; set; }

        public string AggregateType { get; set; }

        public int Version { get; set; }

        public string DomainEventTypeName { get; set; }

        public string Payload { get; set; }

        public string CorrelationId { get; set; }
    }
}

using System;

namespace ZSharp.Framework.Domain
{
    public class DomainEventEntity : BaseDomainEntity
    {
        public Guid AggregateId { get; set; }

        public string AggregateType { get; set; }

        public int Version { get; set; }

        public string DomainEventTypeName { get; set; }

        public string Payload { get; set; }

        public Guid CorrelationId { get; set; }
    }
}

using System;
using ZSharp.Framework.Entities;

namespace ZSharp.Framework.Domain
{
    public class DomainEventEntity : Entity<Guid>
    {
        public Guid AggregateId { get; set; }

        public string AggregateType { get; set; }

        public int Version { get; set; }

        public string DomainEventTypeName { get; set; }

        public string Payload { get; set; }

        public string CorrelationId { get; set; }
    }
}

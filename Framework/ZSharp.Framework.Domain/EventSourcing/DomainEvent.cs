using System;

namespace ZSharp.Framework.Domain
{
    public abstract class DomainEvent : IDomainEvent
    {
        public Guid Id { get; set; }

        public int Version { get; set; }

        public string CorrelationId { get; set; }
    }
}
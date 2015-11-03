using System;
using System.Collections.Generic;

namespace ZSharp.Framework.Domain
{
    public interface IDomainEventRepository : IDisposable
    {
        void SaveEvents(IEnumerable<IDomainEvent> domainEvents, string sourcedTypeName);

        IEnumerable<IDomainEvent> LoadEvents(Type eventSourcedType, Guid id);

        IEnumerable<IDomainEvent> LoadEvents(Type eventSourcedType, Guid id, int version);
    }
}

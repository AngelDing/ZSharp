using System;
using System.Collections.Generic;

namespace ZSharp.Framework.Domain
{
    public interface IDomainEventRepository : IDisposable
    {
        void SaveEvents(IEnumerable<IDomainEvent> domainEvents, string sourcedTypeName);

        IEnumerable<IDomainEvent> LoadEvents(string sourcedTypeName, Guid id);

        IEnumerable<IDomainEvent> LoadEvents(string sourcedTypeName, Guid id, int version);
    }
}

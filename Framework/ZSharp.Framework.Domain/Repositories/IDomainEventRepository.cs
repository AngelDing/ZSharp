using System;
using System.Collections.Generic;
using ZSharp.Framework.Specifications;

namespace ZSharp.Framework.Domain
{
    public interface IDomainEventRepository : IDisposable
    {
        void SaveEvents(IEnumerable<IDomainEvent> domainEvents, string sourcedTypeName);

        IEnumerable<IDomainEvent> LoadEvents(string sourcedTypeName, Guid id);

        IEnumerable<IDomainEvent> LoadEvents(string sourcedTypeName, Guid id, int version);
    }
}

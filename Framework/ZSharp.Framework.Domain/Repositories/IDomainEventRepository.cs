using System;
using System.Collections.Generic;

namespace ZSharp.Framework.Domain
{
    public interface IDomainEventRepository<T> where T : IEventSourced
    {
        void SaveEvents(IEnumerable<IDomainEvent> domainEvents);

        IEnumerable<IDomainEvent> LoadEvents(Guid id);

        IEnumerable<IDomainEvent> LoadEvents(Guid id, int version);
    }
}

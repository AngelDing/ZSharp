using System;
using System.Collections.Generic;
using ZSharp.Framework.Entities;

namespace ZSharp.Framework.Domain
{   
    public interface IEventSourced : IEvent, ISnapshotOrignator, IAggregateRoot
    {
        int Version { get; }

        IEnumerable<IDomainEvent> Events { get; }

        void LoadFromHistory(IEnumerable<IDomainEvent> historyEvents);
    }
}

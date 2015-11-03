
using System;

namespace ZSharp.Framework.Domain
{
    public interface ISnapshotRepository
    {
        ISnapshot GetSnapshot(Type eventSourcedType, Guid id);

        bool CanCreateOrUpdateSnapshot(IEventSourced eventSourced);

        void CreateOrUpdateSnapshot(IEventSourced eventSourced);

        bool HasSnapshot(Type eventSourcedType, Guid id);
    }
}

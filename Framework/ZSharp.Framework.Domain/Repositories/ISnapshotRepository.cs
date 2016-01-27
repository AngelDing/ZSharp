
using System;

namespace ZSharp.Framework.Domain
{
    public interface ISnapshotRepository<T> where T : IEventSourced
    {
        ISnapshot GetSnapshot(Guid id);

        void SaveSnapshot(T eventSourced);

        bool HasSnapshot(Guid id);
    }
}

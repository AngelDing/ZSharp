using System;

namespace ZSharp.Framework.Domain
{
    public class NullSnapshotRepository<T> : ISnapshotRepository<T> where T : IEventSourced
    {
        public ISnapshot GetSnapshot(Guid id)
        {
            return null;
        }

        public bool HasSnapshot(Guid id)
        {
            return false;
        }

        public void SaveSnapshot(T eventSourced)
        {
        }
    }
}

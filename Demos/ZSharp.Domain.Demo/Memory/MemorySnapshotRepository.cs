using ZSharp.Framework.Domain;
using System;

namespace ZSharp.Domain.Demo
{
    public class MemorySnapshotRepository<T> : ISnapshotRepository<T> where T : IEventSourced
    {
        public void SaveSnapshot(T eventSourced)
        {            
        }

        public ISnapshot GetSnapshot(Guid id)
        {
            return null;
        }

        public bool HasSnapshot(Guid id)
        {
            return false;
        }
    }
}

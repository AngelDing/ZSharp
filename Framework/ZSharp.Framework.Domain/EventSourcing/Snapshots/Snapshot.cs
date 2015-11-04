
using System;

namespace ZSharp.Framework.Domain
{
    public abstract class Snapshot : ISnapshot
    {
        public Guid AggregateId { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public int Version { get; set; }

        public string SnapshotData { get; set; }
    }
}

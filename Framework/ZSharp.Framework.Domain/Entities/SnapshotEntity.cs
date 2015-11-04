using System;
using ZSharp.Framework.Entities;

namespace ZSharp.Framework.Domain
{
    public class SnapshotEntity : Entity<long>, ISnapshot
    {
        public Guid AggregateId { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public int Version { get; set; }

        public string SnapshotData { get; set; }

        public string AggregateType { get; set; }

        public string SnapshotType { get; set; }
    }
}

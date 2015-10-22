
using System;

namespace ZSharp.Framework.Domain
{
    public interface ISnapshot
    {
        Guid AggregateId { get; set; }

        DateTimeOffset Timestamp { get; set; }

        int Version { get; set; }

        string SnapshotData { get; set; }
    }
}


using System;
using ZSharp.Framework.DesignPatterns;

namespace ZSharp.Framework.Domain
{
    public interface ISnapshot : IMemento<string>
    {
        Guid AggregateId { get; set; }

        DateTimeOffset Timestamp { get; set; }

        int Version { get; set; }
    }
}

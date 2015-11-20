
using System;

namespace ZSharp.Framework.Domain
{
    public interface ISnapshot
    {
        Guid AggregateId { get; set; }

        int Version { get; set; }
    }
}

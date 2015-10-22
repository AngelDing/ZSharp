using System;

namespace ZSharp.Framework.Domain
{
    public abstract class VersionedEvent : IVersionedEvent
    {
        public Guid Id { get; set; }

        public int Version { get; set; }
    }
}
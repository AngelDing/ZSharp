using System;

namespace ZSharp.Framework.Domain
{
    public interface IEvent
    {
        Guid Id { get; }
    }
}

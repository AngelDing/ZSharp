using System.Collections.Generic;

namespace ZSharp.Framework.Domain
{
    /// <summary>
    /// Defines that the object exposes events that are meant to be published.
    /// </summary>
    public interface IEventPublisher
    {
        IEnumerable<IEvent> Events { get; }
    }
}

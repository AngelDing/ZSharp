using System;
using System.Collections.Generic;
using ZSharp.Framework.Entities;

namespace ZSharp.Framework.Domain
{   
    /// <summary>
    /// Represents an identifiable entity that is event sourced.
    /// </summary>
    public interface IEventSourced : IVersionedEvent, ISnapshotOrignator, IAggregateRoot<Guid>
    {
        /// <summary>
        /// Gets the collection of new events since the entity was loaded, as a consequence of command handling.
        /// </summary>
        IEnumerable<IVersionedEvent> Events { get; }

        /// <summary>
        /// Load the aggreate from the historial events.
        /// </summary>
        /// <param name="pastEvents">The historical events from which the aggregate is built.</param>
        void LoadFromHistory(IEnumerable<IVersionedEvent> pastEvents);
    }
}

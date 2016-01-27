using ZSharp.Framework.Domain;
using ZSharp.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ZSharp.Domain.Demo
{
    public class MemoryDomainEventRepository<T> : IDomainEventRepository<T> where T : IEventSourced
    {
        private struct EventDescriptor
        {
            public readonly string EventSourcedType;
            public readonly IDomainEvent EventData;
            public readonly Guid Id;
            public readonly int Version;

            public EventDescriptor(Guid id, IDomainEvent eventData)
            {
                EventData = eventData;
                EventSourcedType = typeof(T).Name;
                Id = id;
                Version = eventData.Version;
            }
        }

        private static Dictionary<Guid, List<EventDescriptor>> current = new Dictionary<Guid, List<EventDescriptor>>();

        public IEnumerable<IDomainEvent> LoadEvents(Guid id)
        {
            List<EventDescriptor> eventDescriptors;

            if (!current.TryGetValue(id, out eventDescriptors))
            {
                throw ErrorHelper.Argument("id", "Aggregate Not Found");
            }

            return eventDescriptors.Select(desc => desc.EventData).ToList();
        }

        public IEnumerable<IDomainEvent> LoadEvents(Guid id, int version)
        {
            var allEvents = this.LoadEvents(id);
            return allEvents.Where(p => p.Version > version);
        }

        public void SaveEvents(IEnumerable<IDomainEvent> domainEvents)
        {
            var aggregateId = domainEvents.FirstOrDefault().SourceId;

            List<EventDescriptor> eventDescriptors;

            if (!current.TryGetValue(aggregateId, out eventDescriptors))
            {
                eventDescriptors = new List<EventDescriptor>();
                current.Add(aggregateId, eventDescriptors);
            }

            foreach (var @event in domainEvents)
            {
                eventDescriptors.Add(new EventDescriptor(aggregateId, @event));
            }
        }
    }
}

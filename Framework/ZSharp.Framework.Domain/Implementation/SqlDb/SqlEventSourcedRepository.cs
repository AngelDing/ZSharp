using System;
using System.Collections.Generic;
using System.Linq;
using ZSharp.Framework.Serializations;

namespace ZSharp.Framework.Domain
{
    /// <summary>
    /// This is an extremely basic implementation of the event store (straw man), that is used only for running the sample application
    /// without the dependency to the Windows Azure Service Bus when using the DebugLocal solution configuration.
    /// It does not check for event versions before committing, nor is transactional with the event bus nor resilient to connectivity errors or crashes.
    /// It does not do any snapshots either for entities that implement <see cref="IMementoOriginator"/>, which would benefit the usage of SeatsAvailability.
    /// </summary>
    /// <typeparam name="T">The entity type to persist.</typeparam>
    public class SqlEventSourcedRepository<T> : IEventSourcedRepository<T> where T : class, IEventSourced
    {
        // Could potentially use DataAnnotations to get a friendly/unique name in case of collisions between BCs.
        private static readonly string sourceType = typeof(T).Name;
        private readonly IEventBus eventBus;
        private readonly ISerializer serializer;
        private readonly Func<EventStoreDbContext> contextFactory;
        private readonly Func<Guid, IEnumerable<IVersionedEvent>, T> entityFactory;

        public SqlEventSourcedRepository(IEventBus eventBus, ISerializer serializer, Func<EventStoreDbContext> contextFactory)
        {
            this.eventBus = eventBus;
            this.serializer = serializer;
            this.contextFactory = contextFactory;

            // TODO: could be replaced with a compiled lambda
            var constructor = typeof(T).GetConstructor(new[] { typeof(Guid), typeof(IEnumerable<IVersionedEvent>) });
            if (constructor == null)
            {
                throw new InvalidCastException("Type T must have a constructor with the following signature: .ctor(Guid, IEnumerable<IVersionedEvent>)");
            }
            this.entityFactory = (id, events) => (T)constructor.Invoke(new object[] { id, events });
        }

        public T Find(Guid id)
        {
            using (var context = this.contextFactory.Invoke())
            {
                var deserialized = context.Set<EventEntity>()
                    .Where(x => x.AggregateId == id && x.AggregateType == sourceType)
                    .OrderBy(x => x.Version)
                    .AsEnumerable()
                    .Select(this.Deserialize)
                    .AsCachedAnyEnumerable();

                if (deserialized.Any())
                {
                    return entityFactory.Invoke(id, deserialized);
                }

                return null;
            }
        }

        public T Get(Guid id)
        {
            var entity = this.Find(id);
            if (entity == null)
            {
                throw new EntityNotFoundException(id, sourceType);
            }

            return entity;
        }

        public void Save(T eventSourced, string correlationId)
        {
            // TODO: guarantee that only incremental versions of the event are stored
            var events = eventSourced.Events.ToArray();
            using (var context = this.contextFactory.Invoke())
            {
                var eventsSet = context.Set<EventEntity>();
                foreach (var e in events)
                {
                    eventsSet.Add(this.Serialize(e, correlationId));
                }

                context.SaveChanges();
            }

            // TODO: guarantee delivery or roll back, or have a way to resume after a system crash
            this.eventBus.Publish(events.Select(e => new Envelope<IEvent>(e) { CorrelationId = correlationId }));
        }

        private EventEntity Serialize(IVersionedEvent e, string correlationId)
        {
            var versionedEventType = e.GetType().AssemblyQualifiedName;
            var payload = this.serializer.Serialize<string>(e);
            var serialized = new EventEntity
            {
                AggregateId = e.SourceId,
                AggregateType = sourceType,
                Version = e.Version,
                VersionedEventType = versionedEventType,
                Payload = payload,
                CorrelationId = correlationId
            };

            return serialized;
        }

        private IVersionedEvent Deserialize(EventEntity @event)
        {
            var type = Type.GetType(@event.VersionedEventType);
            var result = this.serializer.Deserialize(@event.Payload, type);
            return result as IVersionedEvent;
        }
    }
}
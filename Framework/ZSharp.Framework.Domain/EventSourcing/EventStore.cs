using ZSharp.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ZSharp.Framework.Exceptions;

namespace ZSharp.Framework.Domain
{
    public class EventStore<T> : IEventStore<T> where T : class, IEventSourced
    {
        private static readonly string sourcedTypeName = typeof(T).Name;
        private static readonly Type sourcedType = typeof(T);
        private readonly IEventBus eventBus;
        private readonly ISnapshotRepository<T> snapshotRepo;
        private readonly IDomainEventRepository<T> domainEventRepo;
        private readonly ISnapshotPolicy snapshotPlicy;

        public EventStore(
            IEventBus eventBus,
            IDomainEventRepository<T> domainEventRepo,
            ISnapshotRepository<T> snapshotRepo,
            ISnapshotPolicy snapshotPlicy)
        {
            this.eventBus = eventBus;
            this.snapshotRepo = snapshotRepo;
            this.domainEventRepo = domainEventRepo;
            this.snapshotPlicy = snapshotPlicy;
        }

        public T Get(Guid id)
        {
            T eventSourced = this.CreateEventSourcedInstance();
            IEnumerable<IDomainEvent> domainEvents;
            if (this.snapshotRepo.HasSnapshot(id))
            {
                ISnapshot snapshot = snapshotRepo.GetSnapshot(id);
                eventSourced.LoadFromSnapshot(snapshot);
                var eventsAfterSnapshot = this.domainEventRepo.LoadEvents(id, snapshot.Version);
                domainEvents = eventsAfterSnapshot;
            }
            else
            {
                domainEvents = domainEventRepo.LoadEvents(id);                
            }
            eventSourced.LoadFromHistory(domainEvents);

            return eventSourced;
        }

        public void Save(T eventSourced)
        {
            var correlationId = eventSourced.CorrelationId;
            if (correlationId == Guid.Empty)
            {
                throw ErrorHelper.Argument("eventSourced", "CorrelationId不能为空的Guid");
            }

            if (snapshotPlicy.ShouldCreateSnapshot(eventSourced))
            {
                this.snapshotRepo.SaveSnapshot(eventSourced);
            }

            // TODO: guarantee that only incremental versions of the event are stored
            var events = eventSourced.PendingEvents.ToList();
            events.ForEach(p => p.CorrelationId = correlationId);
            domainEventRepo.SaveEvents(events);

            var topic = eventSourced.Topic;
            var envelopeEvents = events.Select(e => new Envelope<IEvent>(e)
            {
                CorrelationId = e.CorrelationId,
                Topic = topic
            });
            // TODO: guarantee delivery or roll back, or have a way to resume after a system crash
            this.eventBus.Publish(envelopeEvents.ToList());
        }

        private T CreateEventSourcedInstance()
        {
            Type eventSourcedType = typeof(T);
            ConstructorInfo constructor = eventSourcedType
                .GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(p =>
                {
                    var parameters = p.GetParameters();
                    return parameters == null || parameters.Length == 0;
                }).FirstOrDefault();
            if (constructor != null)
            {
                return constructor.Invoke(null) as T;
            }
            throw new DomainException("At least one parameterless constructor should be defined on the aggregate root type '{0}'.", typeof(T));
        }
    }
}

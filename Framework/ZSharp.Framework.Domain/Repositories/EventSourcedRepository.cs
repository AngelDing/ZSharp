using System;
using System.Linq;
using System.Reflection;

namespace ZSharp.Framework.Domain
{
    public class EventSourcedRepository<T> : IEventSourcedRepository<T> where T : class, IEventSourced
    {
        private static readonly string sourcedTypeName = typeof(T).Name;
        private static readonly Type sourcedType = typeof(T);
        private readonly IEventBus eventBus;
        private readonly ISnapshotRepository snapshotRepo;
        private readonly IDomainEventRepository domainEventRepo;

        public EventSourcedRepository(
            IEventBus eventBus,
            IDomainEventRepository domainEventRepo,
            ISnapshotRepository snapshotRepo)
        {
            this.eventBus = eventBus;
            this.snapshotRepo = snapshotRepo;
            this.domainEventRepo = domainEventRepo;
        }

        public T Get(Guid id)
        {
            T eventSourced = this.CreateEventSourcedInstance();
            if (this.snapshotRepo != null && this.snapshotRepo.HasSnapshot(sourcedType, id))
            {
                ISnapshot snapshot = snapshotRepo.GetSnapshot(sourcedType, id);
                eventSourced.LoadFromSnapshot(snapshot);
                var eventsAfterSnapshot = this.domainEventRepo.LoadEvents(sourcedTypeName, id, snapshot.Version);
                if (eventsAfterSnapshot != null && eventsAfterSnapshot.Count() > 0)
                {
                    eventSourced.LoadFromHistory(eventsAfterSnapshot);
                }
            }
            else
            {
                var domainEvents = domainEventRepo.LoadEvents(sourcedTypeName, id);
                eventSourced.LoadFromHistory(domainEvents);
            }
            return eventSourced;
        }

        public void Save(T eventSourced)
        {
            if (this.snapshotRepo != null && this.snapshotRepo.CanCreateOrUpdateSnapshot(eventSourced))
            {
                this.snapshotRepo.CreateOrUpdateSnapshot(eventSourced);
            }

            // TODO: guarantee that only incremental versions of the event are stored
            var events = eventSourced.Events.ToArray();
            domainEventRepo.SaveEvents(events, sourcedTypeName);

            // TODO: guarantee delivery or roll back, or have a way to resume after a system crash
            this.eventBus.Publish(events.Select(e => new Envelope<IEvent>(e) { CorrelationId = e.CorrelationId }));
        }

        protected T CreateEventSourcedInstance()
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

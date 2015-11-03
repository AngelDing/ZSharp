//using System;
//using System.Collections.Generic;
//using System.Linq;
//using ZSharp.Framework.Serializations;

//namespace ZSharp.Framework.Domain
//{
//    public class SqlEventSourcedRepository<T> : EventSourcedRepository<T> where T : class, IEventSourced
//    {
//        private static readonly string sourceTypeName = typeof(T).Name;
//        private static readonly Type sourceType = typeof(T);
//        private readonly IEventBus eventBus;
//        private readonly ISerializer serializer;
//        private readonly Func<DomainDbContext> contextFactory;
//        private readonly ISnapshotRepository snapshotRepo;

//        public SqlEventSourcedRepository(
//            IEventBus eventBus, 
//            ISerializer serializer,
//            Func<DomainDbContext> contextFactory,
//            ISnapshotRepository snapshotRepo = null)
//        {
//            this.eventBus = eventBus;
//            this.serializer = serializer;
//            this.snapshotRepo = snapshotRepo;
//            this.contextFactory = contextFactory;
//        }

//        public override T Get(Guid id)
//        {
//            T eventSourced = this.CreateEventSourcedInstance();
//            if (this.snapshotRepo != null && this.snapshotRepo.HasSnapshot(sourceType, id))
//            {
//                ISnapshot snapshot = snapshotRepo.GetSnapshot(sourceType, id);
//                eventSourced.LoadFromSnapshot(snapshot);
//                //var eventsAfterSnapshot = this.domainEventStorage.LoadEvents(typeof(TAggregateRoot), id, snapshot.Version);
//                //if (eventsAfterSnapshot != null && eventsAfterSnapshot.Count() > 0)
//                //    aggregateRoot.BuildFromHistory(eventsAfterSnapshot);
//            }
//            else
//            {
//                using (var context = this.contextFactory.Invoke())
//                {
//                    var deserialized = context.Set<DomainEventEntity>()
//                        .Where(x => x.AggregateId == id && x.AggregateType == sourceTypeName)
//                        .OrderBy(x => x.Version)
//                        .AsEnumerable()
//                        .Select(this.Deserialize)
//                        .AsCachedAnyEnumerable();

//                    if (deserialized.Any())
//                    {
//                        eventSourced.LoadFromHistory(deserialized);
//                    }
//                }
//            }
//            return eventSourced;
//        }

//        public override void Save(T eventSourced)
//        {
//            if (this.snapshotRepo != null && this.snapshotRepo.CanCreateOrUpdateSnapshot(eventSourced))
//            {
//                this.snapshotRepo.CreateOrUpdateSnapshot(eventSourced);
//            }

//            // TODO: guarantee that only incremental versions of the event are stored
//            var events = eventSourced.Events.ToArray();
//            using (var context = this.contextFactory.Invoke())
//            {
//                var eventsSet = context.Set<DomainEventEntity>();
//                foreach (var e in events)
//                {
//                    eventsSet.Add(this.Serialize(e));
//                }

//                context.SaveChanges();
//            }

//            // TODO: guarantee delivery or roll back, or have a way to resume after a system crash
//            this.eventBus.Publish(events.Select(e => new Envelope<IEvent>(e) { CorrelationId = e.CorrelationId }));
//        }

//        private DomainEventEntity Serialize(IDomainEvent e)
//        {
//            var versionedEventType = e.GetType().AssemblyQualifiedName;
//            var payload = this.serializer.Serialize<string>(e);
//            var serialized = new DomainEventEntity
//            {
//                AggregateId = e.Id,
//                AggregateType = sourceTypeName,
//                Version = e.Version,
//                DomainEventTypeName = versionedEventType,
//                Payload = payload,
//                CorrelationId = e.CorrelationId
//            };

//            return serialized;
//        }

//        private IDomainEvent Deserialize(DomainEventEntity @event)
//        {
//            var type = Type.GetType(@event.DomainEventTypeName);
//            var result = this.serializer.Deserialize(@event.Payload, type);
//            return result as IDomainEvent;
//        }
//    }
//}
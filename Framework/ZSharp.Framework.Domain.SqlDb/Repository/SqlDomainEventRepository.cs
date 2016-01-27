using System;
using System.Linq;
using System.Collections.Generic;
using ZSharp.Framework.Serializations;
using ZSharp.Framework.EfExtensions;

namespace ZSharp.Framework.Domain
{
    public class SqlDomainEventRepository<T> : BaseSqlDomainRepositroy, IDomainEventRepository<T> where T : IEventSourced
    {
        private readonly ISerializer serializer;
        private readonly string sourcedTypeName;

        public SqlDomainEventRepository(ISerializer serializer)
        {
            this.serializer = serializer;
            sourcedTypeName = typeof(T).Name;
        }

        public IEnumerable<IDomainEvent> LoadEvents(Guid id)
        {
            var entities = DB.DomainEvents
                .Where(p => p.AggregateType == sourcedTypeName && p.Id == id)
                .OrderBy(p => p.Version)
                .ToList();

            var eventList = new List<IDomainEvent>();
            entities.ForEach(p => eventList.Add(this.Deserialize(p)));

            return eventList;
        }

        public IEnumerable<IDomainEvent> LoadEvents(Guid id, int version)
        {
            var entities = DB.DomainEvents
               .Where(p => p.AggregateType == sourcedTypeName && p.Id == id && p.Version > version)
               .OrderBy(p => p.Version)
               .ToList();

            var eventList = new List<IDomainEvent>();
            entities.ForEach(p => eventList.Add(this.Deserialize(p)));

            return eventList;
        }

        public void SaveEvents(IEnumerable<IDomainEvent> domainEvents)
        {
            var entities = new List<DomainEventEntity>();
            domainEvents.ToList().ForEach(p => entities.Add(this.Serialize(p)));
            DB.BulkInsert(entities);
        }

        private DomainEventEntity Serialize(IDomainEvent @event)
        {
            var versionedEventType = @event.GetType().AssemblyQualifiedName;
            var payload = this.serializer.Serialize<string>(@event);
            var serialized = new DomainEventEntity
            {
                AggregateId = @event.SourceId,
                AggregateType = sourcedTypeName,
                Version = @event.Version,
                DomainEventTypeName = versionedEventType,
                Payload = payload,
                CorrelationId = @event.CorrelationId
            };

            return serialized;
        }

        private IDomainEvent Deserialize(DomainEventEntity @event)
        {
            var type = Type.GetType(@event.DomainEventTypeName);
            var result = this.serializer.Deserialize(@event.Payload, type);
            return result as IDomainEvent;
        }
    }
}

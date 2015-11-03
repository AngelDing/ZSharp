using System;
using System.Linq;
using System.Collections.Generic;
using ZSharp.Framework.Serializations;
using ZSharp.Framework.EfExtensions;

namespace ZSharp.Framework.Domain
{
    public class SqlDomainEventRepository : DisposableObject, IDomainEventRepository
    {
        private readonly DomainDbContext db;
        private readonly ISerializer serializer;

        public SqlDomainEventRepository(ISerializer serializer)
        {
            this.db = new DomainDbContext();
            this.serializer = serializer;
        }

        public IEnumerable<IDomainEvent> LoadEvents(string sourcedTypeName, Guid id)
        {
            var entities = db.DomainEvents
                .Where(p => p.AggregateType == sourcedTypeName && p.Id == id)
                .OrderBy(p => p.Version)
                .ToList();

            var eventList = new List<IDomainEvent>();
            entities.ForEach(p => eventList.Add(this.Deserialize(p)));

            return eventList;
        }

        public IEnumerable<IDomainEvent> LoadEvents(string sourcedTypeName, Guid id, int version)
        {
            var entities = db.DomainEvents
               .Where(p => p.AggregateType == sourcedTypeName && p.Id == id && p.Version > version)
               .OrderBy(p => p.Version)
               .ToList();

            var eventList = new List<IDomainEvent>();
            entities.ForEach(p => eventList.Add(this.Deserialize(p)));

            return eventList;
        }

        public void SaveEvents(IEnumerable<IDomainEvent> domainEvents, string sourcedTypeName)
        {
            var entities = new List<DomainEventEntity>();
            domainEvents.ToList().ForEach(p => entities.Add(this.Serialize(p, sourcedTypeName)));
            db.BulkInsert(entities);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.db.Dispose();
            }
        }

        private DomainEventEntity Serialize(IDomainEvent @event, string sourcedTypeName)
        {
            var versionedEventType = @event.GetType().AssemblyQualifiedName;
            var payload = this.serializer.Serialize<string>(@event);
            var serialized = new DomainEventEntity
            {
                AggregateId = @event.Id,
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

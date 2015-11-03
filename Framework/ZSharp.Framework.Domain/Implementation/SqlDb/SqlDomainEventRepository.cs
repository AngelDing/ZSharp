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

        public IEnumerable<IDomainEvent> LoadEvents(Type eventSourcedType, Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IDomainEvent> LoadEvents(Type eventSourcedType, Guid id, int version)
        {
            throw new NotImplementedException();
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
    }
}

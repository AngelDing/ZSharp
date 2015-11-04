using System;
using System.Linq;
using ZSharp.Framework.Extensions;
using ZSharp.Framework.EfExtensions;
using ZSharp.Framework.Serializations;

namespace ZSharp.Framework.Domain
{
    public class SqlSnapshotRepository : ISnapshotRepository
    {
        private readonly DomainDbContext db;
        private readonly int numOfEvents;
        private readonly ISerializer serializer;
        private readonly IDomainEventRepository domainEventRepo;

        public SqlSnapshotRepository(IDomainEventRepository domainEventRepo, ISerializer serializer)
        {
            this.numOfEvents = 5;
            this.db = new DomainDbContext();
            this.serializer = serializer;
            this.domainEventRepo = domainEventRepo;
        }

        public bool CanCreateOrUpdateSnapshot(IEventSourced eventSourced)
        {
            bool isOk;
            var eventSourcedType = eventSourced.GetType();
            var id = eventSourced.Id;
            if (this.HasSnapshot(eventSourcedType, id))
            {
                ISnapshot snapshot = this.GetSnapshot(eventSourcedType, id);
                isOk = snapshot.Version + numOfEvents <= eventSourced.Version;
            }
            else
            {
                var eventCount = eventSourced.Version + 1;
                isOk = eventCount >= this.numOfEvents;
            }
            return isOk;
        }

        public void CreateOrUpdateSnapshot(IEventSourced eventSourced)
        {
            var eventSourcedType = eventSourced.GetType();
            var id = eventSourced.Id;
            var entity = CreateFromEventSourced(eventSourced);

            if (this.HasSnapshot(eventSourcedType, id))
            {
                db.Update(entity);
            }
            else
            {
                db.Insert(entity);
            }
        }

        public ISnapshot GetSnapshot(Type eventSourcedType, Guid id)
        {
            string eventSourcedTypeName = eventSourcedType.Name;
            var entity = db.Snapshots
                .FirstOrDefault(p => p.AggregateType == eventSourcedTypeName && p.AggregateId == id);

            if (entity == null)
            {
                return null;
            }
            ISnapshot snapshot = ExtractSnapshot(entity);
            return snapshot;
        }

        public bool HasSnapshot(Type eventSourcedType, Guid id)
        {
            string eventSourcedTypeName = eventSourcedType.Name;
            var entities = db.Snapshots
                .Where(p => p.AggregateType == eventSourcedTypeName && p.AggregateId == id);
            var isHas = false;
            if (!entities.IsNullOrEmpty())
            {
                isHas = true;
            }
            return isHas;
        }

        #region Private Methods

        private SnapshotEntity CreateFromEventSourced(IEventSourced eventSourced)
        {
            ISnapshot snapshot = eventSourced.CreateSnapshot();

            return new SnapshotEntity
            {
                AggregateId = eventSourced.Id,
                AggregateType = eventSourced.GetType().Name,
                Version = eventSourced.Version,
                SnapshotType = snapshot.GetType().AssemblyQualifiedName,
                Timestamp = snapshot.Timestamp,
                SnapshotData = serializer.Serialize<string>(snapshot)
            };
        }

        private ISnapshot ExtractSnapshot(SnapshotEntity entity)
        {
            Type snapshotType = Type.GetType(entity.SnapshotType);
            if (snapshotType == null)
            {
                return null;
            }
            return (ISnapshot)serializer.Deserialize(entity.SnapshotData, snapshotType);
        }

        #endregion
    }
}

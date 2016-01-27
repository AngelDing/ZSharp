using System;
using System.Linq;
using ZSharp.Framework.Extensions;
using ZSharp.Framework.EfExtensions;
using ZSharp.Framework.Serializations;

namespace ZSharp.Framework.Domain
{
    public class SqlSnapshotRepository<T> : BaseSqlDomainRepositroy, ISnapshotRepository<T> where T : IEventSourced
    {
        private readonly ISerializer serializer;
        private readonly IDomainEventRepository<T> domainEventRepo;
        private readonly Type eventSourcedType;

        public SqlSnapshotRepository(IDomainEventRepository<T> domainEventRepo, ISerializer serializer)
        {
            this.serializer = serializer;
            this.domainEventRepo = domainEventRepo;
            eventSourcedType = typeof(T);
        }

        public void SaveSnapshot(T eventSourced)
        {
            var id = eventSourced.Id;
            var entity = CreateFromEventSourced(eventSourced);

            if (this.HasSnapshot(id))
            {
                DB.Update(entity);
            }
            else
            {
                DB.Insert(entity);
            }
        }

        public ISnapshot GetSnapshot(Guid id)
        {
            string eventSourcedTypeName = eventSourcedType.Name;
            var entity = DB.Snapshots
                .FirstOrDefault(p => p.AggregateType == eventSourcedTypeName && p.AggregateId == id);

            if (entity == null)
            {
                return null;
            }
            ISnapshot snapshot = ExtractSnapshot(entity);
            return snapshot;
        }

        public bool HasSnapshot(Guid id)
        {
            string eventSourcedTypeName = eventSourcedType.Name;
            var entities = DB.Snapshots
                .Where(p => p.AggregateType == eventSourcedTypeName && p.AggregateId == id);
            var isHas = false;
            if (!entities.IsNullOrEmpty())
            {
                isHas = true;
            }
            return isHas;
        }

        #region Private Methods

        private SnapshotEntity CreateFromEventSourced(T eventSourced)
        {
            ISnapshot snapshot = eventSourced.CreateSnapshot();

            return new SnapshotEntity
            {
                AggregateId = eventSourced.Id,
                AggregateType = eventSourced.GetType().Name,
                Version = eventSourced.Version,
                SnapshotType = snapshot.GetType().AssemblyQualifiedName,
                Timestamp = DateTimeOffset.Now,
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

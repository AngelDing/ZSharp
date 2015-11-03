using System;

namespace ZSharp.Framework.Domain
{
    public class SnapshotRepository : ISnapshotRepository
    {
        private readonly int numOfEvents;

        public SnapshotRepository()
        {
            this.numOfEvents = 5;
        }

        public bool CanCreateOrUpdateSnapshot(IEventSourced eventSourced)
        {
            var eventSourcedType = eventSourced.GetType();
            var id = eventSourced.Id;
            if (this.HasSnapshot(eventSourcedType, id))
            {
                ISnapshot snapshot = this.GetSnapshot(eventSourcedType, id);
                return snapshot.Version + numOfEvents < eventSourced.Version;
            }
            else
            {
                //string eventSourcedTypeName = eventSourcedType.AssemblyQualifiedName;
                //var version = eventSourced.Version;

                //ISpecification<DomainEventDataObject> spec = Specification<DomainEventDataObject>.Eval(
                //    p => p.SourceID == aggregateRootId &&
                //        p.AssemblyQualifiedSourceType == aggregateRootTypeName &&
                //        p.Version <= version);
                //int eventCnt = this.EventStorage.GetRecordCount<DomainEventDataObject>(spec);
                //return eventCnt >= this.numOfEvents;
            }
            return true;
        }

        public void CreateOrUpdateSnapshot(IEventSourced eventSourced)
        {
            throw new NotImplementedException();
        }

        public ISnapshot GetSnapshot(Type eventSourcedType, Guid id)
        {
            throw new NotImplementedException();
        }

        public bool HasSnapshot(Type eventSourcedType, Guid id)
        {
            throw new NotImplementedException();
        }
    }
}

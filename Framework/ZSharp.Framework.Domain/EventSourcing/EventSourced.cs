using System;
using System.Collections.Generic;
using ZSharp.Framework.Extensions;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.Domain
{
    public abstract class EventSourced : IEventSourced
    {
        private readonly Dictionary<Type, Action<IDomainEvent>> handlers = new Dictionary<Type, Action<IDomainEvent>>();
        private readonly List<IDomainEvent> pendingEvents = new List<IDomainEvent>();

        protected EventSourced()
            : this(GuidHelper.NewSequentialId())
        {
        }

        protected EventSourced(Guid id)
        {
            this.Id = id;
            this.Version = Constants.ApplicationRuntime.DefaultVersion;
        }

        public Guid Id { get; set; }

        public int Version { get; protected set; }

        public IEnumerable<IDomainEvent> Events
        {
            get { return this.pendingEvents; }
        }

        protected void Handles<TEvent>(Action<TEvent> handler)
            where TEvent : IEvent
        {
            this.handlers.Add(typeof(TEvent), @event => handler((TEvent)@event));
        }

        public void LoadFromHistory(IEnumerable<IDomainEvent> historyEvents)
        {
            if (historyEvents.IsNullOrEmpty())
            {
                return;
            }

            foreach (var e in historyEvents)
            {
                this.handlers[e.GetType()].Invoke(e);
                this.Version = e.Version;
            }
        }

        protected void Update(DomainEvent e)
        {
            e.Id = this.Id;
            e.Version = this.Version + 1;
            this.handlers[e.GetType()].Invoke(e);
            this.Version = e.Version;
            this.pendingEvents.Add(e);
        }

        protected abstract void DoLoadFromSnapshot(ISnapshot snapshot);

        protected abstract ISnapshot DoCreateSnapshot();

        #region IOrignator Members

        public virtual void LoadFromSnapshot(ISnapshot snapshot)
        {
            this.Version = snapshot.Version;
            this.Id = snapshot.AggregateId;
            DoLoadFromSnapshot(snapshot);
            this.pendingEvents.Clear();
        }

        public virtual ISnapshot CreateSnapshot()
        {
            ISnapshot snapshot = this.DoCreateSnapshot();

            snapshot.Version = this.Version;
            snapshot.Timestamp = DateTimeOffset.Now;
            snapshot.AggregateId = this.Id;
            return snapshot;
        }
        #endregion
    }
}

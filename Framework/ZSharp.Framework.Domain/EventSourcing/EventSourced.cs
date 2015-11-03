using System;
using System.Collections.Generic;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.Domain
{
    /// <summary>
    /// Base class for event sourced entities that implements.
    /// </summary>
    public abstract class EventSourced : IEventSourced
    {
        private readonly Dictionary<Type, Action<IVersionedEvent>> handlers = new Dictionary<Type, Action<IVersionedEvent>>();
        private readonly List<IVersionedEvent> pendingEvents = new List<IVersionedEvent>();

        public EventSourced()
            : this(GuidHelper.NewSequentialId())
        {
        }

        protected EventSourced(Guid id)
        {
            this.Id = id;
            this.Version = Constants.ApplicationRuntime.DefaultVersion;
        }

        public Guid Id { get; set; }

        public int Version { get; private set; }

        public IEnumerable<IVersionedEvent> Events
        {
            get { return this.pendingEvents; }
        }

        protected void Handles<TEvent>(Action<TEvent> handler)
            where TEvent : IEvent
        {
            this.handlers.Add(typeof(TEvent), @event => handler((TEvent)@event));
        }

        public void LoadFromHistory(IEnumerable<IVersionedEvent> pastEvents)
        {
            foreach (var e in pastEvents)
            {
                this.handlers[e.GetType()].Invoke(e);
                this.Version = e.Version;
            }
        }

        protected void Update(VersionedEvent e)
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

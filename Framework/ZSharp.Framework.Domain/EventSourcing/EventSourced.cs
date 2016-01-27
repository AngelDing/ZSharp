using System;
using System.Collections.Generic;
using ZSharp.Framework.Extensions;

namespace ZSharp.Framework.Domain
{
    public abstract class EventSourced : IEventSourced
    {
        private readonly Dictionary<Type, Action<IDomainEvent>> handlers = new Dictionary<Type, Action<IDomainEvent>>();
        private readonly List<IDomainEvent> pendingEvents = new List<IDomainEvent>();
        private int version = Constants.ApplicationRuntime.DefaultVersion;

        protected EventSourced(Guid id)
        {
            this.Id = id;
        }

        public IEnumerable<IDomainEvent> PendingEvents
        {
            get { return this.pendingEvents; }
        }

        public Guid Id { get; private set; }

        public int Version
        {
            get
            {
                return this.version;
            }
            protected set
            {
                this.version = value;
            }
        }

        public Guid CorrelationId { get; set; }

        public string Topic { get; set; }

        protected void Handles<TEvent>(Action<TEvent> handler) where TEvent : IEvent
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
                this.version = e.Version;
            }
        }

        /// <summary>
        /// 更新领域实体
        /// </summary>
        /// <param name="e">领域事件</param>
        protected void Update(DomainEvent e)
        {
            e.SourceId = this.Id;
            e.Version = this.version + 1;
            this.handlers[e.GetType()].Invoke(e);
            this.version = e.Version;
            this.pendingEvents.Add(e);
        }

        protected abstract void DoLoadFromSnapshot(ISnapshot snapshot);

        protected abstract ISnapshot DoCreateSnapshot();

        #region IOrignator Members

        public virtual void LoadFromSnapshot(ISnapshot snapshot)
        {
            this.version = snapshot.Version;
            this.Id = snapshot.AggregateId;
            DoLoadFromSnapshot(snapshot);
            this.pendingEvents.Clear();
        }

        public virtual ISnapshot CreateSnapshot()
        {
            ISnapshot snapshot = this.DoCreateSnapshot();
            snapshot.Version = this.version;
            snapshot.AggregateId = this.Id;
            return snapshot;
        }

        #endregion
    }
}

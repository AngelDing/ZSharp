using System.Collections.Generic;

namespace ZSharp.Framework.Domain
{
    public class EventBus : IEventBus
    {
        private readonly IMessageSender sender;

        public EventBus(IMessageSender sender)
        {
            this.sender = sender;
        }

        public void Publish(Envelope<IEvent> @event)
        {
            this.sender.Send(@event);
        }

        public void Publish(IEnumerable<Envelope<IEvent>> events)
        {
            this.sender.Send(events);
        }
    }
}

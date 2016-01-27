using System.Collections.Generic;
using ZSharp.Framework.Extensions;
using System.Linq;

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
            InitEvent(@event);
            this.sender.Send(@event);
        }

        public void Publish(IEnumerable<Envelope<IEvent>> events)
        {
            InitEvents(events);
            this.sender.Send(events);
        }

        private void InitEvents(IEnumerable<Envelope<IEvent>> events)
        {
            foreach (var e in events)
            {
                InitEvent(e);
            }
        }

        private void InitEvent(Envelope<IEvent> @event)
        {
            if (@event.Topic.IsNullOrEmpty())
            {
                @event.Topic = Constants.ApplicationRuntime.DefaultEventTopic;
            }
        }
    }

    /// <summary>
    /// 客户端应该定义一套自己的Topic（队列，路由等）
    /// </summary>
    public static class EventBusExtensions
    {
        public static void Publish(this IEventBus bus, IEvent @event, string topic)
        {
            bus.Publish(CreateEnvelopeEvent(@event, topic));
        }

        public static void Publish(this IEventBus bus, IEnumerable<IEvent> events, string topic)
        {
            bus.Publish(events.Select(x => CreateEnvelopeEvent(x, topic)));
        }

        private static Envelope<IEvent> CreateEnvelopeEvent(IEvent @event, string topic)
        {
            var envelopeEvent = new Envelope<IEvent>(@event);
            envelopeEvent.Topic = topic;

            return envelopeEvent;
        }
    }
}

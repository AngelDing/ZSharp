using System.Collections.Generic;
using System.Linq;
using ZSharp.Framework.Configurations;
using ZSharp.Framework.Extensions;

namespace ZSharp.Framework.Domain
{
    public static class EventBusExtensions
    {
        public static void Publish(this IEventBus bus, IEvent @event, string topic = null, string sysCode = null)
        {           
            bus.Publish(CreateEnvelopeEvent(@event, topic, sysCode));
        }

        public static void Publish(this IEventBus bus, IEnumerable<IEvent> events, string topic = null, string sysCode = null)
        {
            bus.Publish(events.Select(x => CreateEnvelopeEvent(x, topic, sysCode)));
        }

        private static Envelope<IEvent> CreateEnvelopeEvent(IEvent @event, string topic, string sysCode)
        {
            if (sysCode.IsNullOrEmpty())
            {
                sysCode = CommonConfig.SystemCode;
            }
            if (topic.IsNullOrEmpty())
            {
                topic = Constants.ApplicationRuntime.DefaultTopic;
            }
            var envelopeEvent = new Envelope<IEvent>(@event);
            envelopeEvent.SysCode = sysCode;
            envelopeEvent.Topic = topic;

            return envelopeEvent;
        }
    }
}

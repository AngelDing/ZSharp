using ZSharp.Framework.Serializations;

namespace ZSharp.Framework.Domain
{
    /// <summary>
    /// Processes incoming events from the bus and routes them to the appropriate 
    /// handlers.
    /// </summary>
    public class EventProcessor : MessageProcessor, IEventHandlerRegistry
    {
        private EventDispatcher messageDispatcher;

        public EventProcessor(IMessageReceiver receiver, ISerializer serializer)
            : base(receiver, serializer)
        {
            this.messageDispatcher = new EventDispatcher();
        }

        public void Register(IEventHandler eventHandler)
        {
            this.messageDispatcher.Register(eventHandler);
        }

        protected override void ProcessMessage(object payload, string correlationId)
        {
            var @event = (IEvent)payload;
            this.messageDispatcher.DispatchMessage(@event, null, correlationId, "");
        }
    }
}

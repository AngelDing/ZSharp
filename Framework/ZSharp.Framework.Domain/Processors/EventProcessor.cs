
namespace ZSharp.Framework.Domain
{
    public class EventProcessor : MessageProcessor
    {
        protected EventProcessor(IMessageReceiver receiver)
            : base(receiver)
        {
        }

        public void Register(IEventHandler handler)
        {
            this.RegisterHandler(handler);
        }
    }
}
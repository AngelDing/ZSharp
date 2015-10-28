
namespace ZSharp.Framework.Domain
{
    public class CommandProcessor : MessageProcessor
    {
        protected CommandProcessor(IMessageReceiver receiver)
            : base(receiver)
        {
        }

        public void Register(ICommandHandler handler)
        {
            this.RegisterHandler(handler);
        }
    }
}

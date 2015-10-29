using System.Collections.Generic;

namespace ZSharp.Framework.Domain
{
    public class CommandBus : ICommandBus
    {
        private readonly IMessageSender sender;

        public CommandBus(IMessageSender sender)
        {
            this.sender = sender;
        }

        public void Send(Envelope<ICommand> command)
        {
            this.sender.Send(command);
        }

        public void Send(IEnumerable<Envelope<ICommand>> commands)
        {
            this.sender.Send(commands);
        }
    }
}

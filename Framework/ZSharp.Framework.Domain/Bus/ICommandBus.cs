using System.Collections.Generic;

namespace ZSharp.Framework.Domain
{  
    public interface ICommandBus : IBus
    {
        void Send(Envelope<ICommand> command);

        void Send(IEnumerable<Envelope<ICommand>> commands);
    }
}

using System.Collections.Generic;

namespace ZSharp.Framework.Domain
{
    public interface IBus
    {
    }

    public interface ICommandBus : IBus
    {
        void Send(Envelope<ICommand> command);

        void Send(IEnumerable<Envelope<ICommand>> commands);
    }

    public interface IEventBus : IBus
    {
        void Publish(Envelope<IEvent> @event);

        void Publish(IEnumerable<Envelope<IEvent>> events);
    }
}

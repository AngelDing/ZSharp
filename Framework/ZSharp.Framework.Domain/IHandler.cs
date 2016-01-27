
namespace ZSharp.Framework.Domain
{
    public interface IHandler { }

    public interface IHandlerRegistry
    {
        void Register(IHandler handler);
    }

    public interface ICommandHandler : IHandler { }

    public interface ICommandHandler<T> : ICommandHandler, IHandler<T> where T : ICommand { }

    public interface IHandler<in T> : IHandler
    {
        void Handle(T message);
    }

    public interface IEventHandler : IHandler { }

    public interface IEventHandler<T> : IEventHandler, IHandler<T> where T : IEvent { }

    public interface IEnvelopedEventHandler<T> : IEventHandler where T : IEvent
    {
        void Handle(Envelope<T> envelope);
    }
}

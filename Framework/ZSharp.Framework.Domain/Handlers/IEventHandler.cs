namespace ZSharp.Framework.Domain
{
    public interface IEventHandler : IHandler
    {
    }

    public interface IEventHandler<T> : IEventHandler, IHandler<T>
        where T : IEvent
    {
    }

    public interface IEnvelopedEventHandler<T> : IEventHandler
        where T : IEvent
    {
        void Handle(Envelope<T> envelope);
    }
}

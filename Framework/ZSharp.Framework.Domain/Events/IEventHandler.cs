namespace ZSharp.Framework.Domain
{
    /// <summary>
    /// Marker interface that makes it easier to discover handlers via reflection.
    /// </summary>
    public interface IEventHandler { }

    public interface IEventHandler<T> : IEventHandler
        where T : IEvent
    {
        void Handle(T @event);
    }

    public interface IEnvelopedEventHandler<T> : IEventHandler
        where T : IEvent
    {
        void Handle(Envelope<T> envelope);
    }
}

namespace ZSharp.Framework.Domain
{
    public interface IEventHandler : IHandler
    {
    }

    public interface IEventHandler<T> : IEventHandler, IHandler<T>
        where T : IEvent
    {
    }
}

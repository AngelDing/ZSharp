namespace ZSharp.Framework.Domain
{
    public interface IEventHandler
    {
    }

    public interface IEventHandler<T> : IEventHandler, IHandler<T>
        where T : IEvent
    {
    }
}

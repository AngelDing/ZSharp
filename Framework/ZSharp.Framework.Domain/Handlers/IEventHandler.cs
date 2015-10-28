namespace ZSharp.Framework.Domain
{
    public interface IEventHandler<T> : IHandler<T>
        where T : IEvent
    {
    }
}

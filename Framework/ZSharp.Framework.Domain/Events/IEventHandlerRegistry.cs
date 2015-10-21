namespace ZSharp.Framework.Domain
{
    public interface IEventHandlerRegistry
    {
        void Register(IEventHandler handler);
    }
}

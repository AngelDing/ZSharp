
namespace ZSharp.Framework.Domain
{
    public interface IMessageDispatcher : IHandlerRegistry
    {
        void Clear();

        void DispatchMessage<T>(T message) where T : IMessage;
    }
}

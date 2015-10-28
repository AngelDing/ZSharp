
namespace ZSharp.Framework.Domain
{
    public interface IHandler
    {
    }

    public interface IHandler<in T> : IHandler
    {
        void Handle(T message);
    }
}

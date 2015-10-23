
namespace ZSharp.Framework.Domain
{
    public interface IHandler<in T>
    {
        void Handle(T message);
    }
}

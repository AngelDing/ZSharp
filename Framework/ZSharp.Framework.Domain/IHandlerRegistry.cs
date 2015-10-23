
namespace ZSharp.Framework.Domain
{
    public interface IHandlerRegistry
    {
        void Register<T>(IHandler<T> handler);

        void UnRegister<T>(IHandler<T> handler);
    }
}


namespace ZSharp.Framework.Domain
{
    public interface ICommandHandlerRegistry
    {
        void Register(ICommandHandler handler);
    }
}


namespace ZSharp.Framework.Domain
{
    public interface ICommandHandler : IHandler
    {
    }

    public interface ICommandHandler<T> : ICommandHandler, IHandler<T>
		where T : ICommand
	{
	}
}

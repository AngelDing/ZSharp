
namespace ZSharp.Framework.Domain
{
	public interface ICommandHandler
    {
    }

	public interface ICommandHandler<T> : ICommandHandler, IHandler<T>
		where T : ICommand
	{
	}
}

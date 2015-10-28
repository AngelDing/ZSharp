
namespace ZSharp.Framework.Domain
{
	public interface ICommandHandler<T> : IHandler<T>
		where T : ICommand
	{
	}
}

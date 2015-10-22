using System;

namespace ZSharp.Framework.Domain
{
	public interface ICommand
    {
		Guid Id { get; }
    }
}

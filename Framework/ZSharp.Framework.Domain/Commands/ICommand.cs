using System;

namespace ZSharp.Framework.Domain
{
	public interface ICommand
    {
		/// <summary>
		/// Gets the command identifier.
		/// </summary>
		Guid Id { get; }
    }
}

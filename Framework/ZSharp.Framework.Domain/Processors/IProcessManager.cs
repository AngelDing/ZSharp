using System;
using System.Collections.Generic;

namespace ZSharp.Framework.Domain
{
    /// <summary>
    /// Interface implemented by process managers (also known as Sagas in the CQRS community) that 
    /// publish commands to the command bus.
    /// </summary>
    /// <remarks>
    /// <para>See <see cref="http://go.microsoft.com/fwlink/p/?LinkID=258564">Reference 6</see> for a description of what is a Process Manager.</para>
    /// <para>There are a few things that we learnt along the way regarding Process Managers, which we might do differently with the new insights that we
    /// now have. See <see cref="http://go.microsoft.com/fwlink/p/?LinkID=258558"> Journey lessons learnt</see> for more information.</para>
    /// </remarks>
    public interface IProcessManager<TEntity> : IProcessManager
    {
        TEntity ProcessEntity { get; set; }
    }

    public interface IProcessManager
    {
        /// <summary>
        /// Gets the process manager identifier.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets a value indicating whether the process manager workflow is completed and the state can be archived.
        /// </summary>
        bool Completed { get; }

        /// <summary>
        /// Gets a collection of commands that need to be sent when the state of the process manager is persisted.
        /// </summary>
        IEnumerable<Envelope<ICommand>> Commands { get; }

        /// <summary>
        /// 持久化到DB
        /// </summary>
        void Save();
    }
}

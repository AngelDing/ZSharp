using System.Threading;
using System.Threading.Tasks;

namespace ZSharp.Framework.Repositories
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Gets a bool value which indicates
        /// whether the Unit of Work could support Microsoft Distributed
        /// Transaction Coordinator (MS-DTC).
        /// </summary>
        bool DistributedTransactionSupported { get; }

        /// <summary>
        /// Gets a bool value which indicates
        /// whether the Unit of Work was successfully committed.
        /// </summary>
        bool Committed { get; }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        void Commit();

        /// <summary>
        /// Commits the transaction asynchronously.
        /// </summary>
        /// <returns>The task that performs the commit operation.</returns>
        Task CommitAsync();

        /// <summary>
        /// Commits the transaction asynchronously.
        /// </summary>
        /// <param name="cancellationToken">
        /// The object which propagates notification that operations should be canceled.
        /// </param>
        /// <returns>The task that performs the commit operation.</returns>
        Task CommitAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Rollback the transaction.
        /// </summary>
        void Rollback();
    }
}

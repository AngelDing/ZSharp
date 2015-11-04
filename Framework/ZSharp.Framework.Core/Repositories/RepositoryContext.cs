using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using ZSharp.Framework.Entities;

namespace ZSharp.Framework.Repositories
{
    /// <summary>
    /// Represents the repository context.
    /// </summary>
    public abstract class RepositoryContext : DisposableObject, IRepositoryContext
    {
        #region Private Fields
        private readonly ConcurrentDictionary<object, byte> newCollection = new ConcurrentDictionary<object, byte>();
        private ConcurrentDictionary<object, byte> modifiedCollection = new ConcurrentDictionary<object, byte>();
        private ConcurrentDictionary<object, byte> deletedCollection = new ConcurrentDictionary<object, byte>();
        private volatile bool committed = true;
        #endregion

        #region Protected Properties
        /// <summary>
        /// Gets an enumerator which iterates over the collection that contains all the objects need to be added to the repository.
        /// </summary>
        protected ConcurrentDictionary<object, byte> NewCollection
        {
            get { return newCollection; }
        }
        /// <summary>
        /// Gets an enumerator which iterates over the collection that contains all the objects need to be modified in the repository.
        /// </summary>
        protected ConcurrentDictionary<object, byte> ModifiedCollection
        {
            get { return modifiedCollection; }
        }
        /// <summary>
        /// Gets an enumerator which iterates over the collection that contains all the objects need to be deleted from the repository.
        /// </summary>
        protected ConcurrentDictionary<object, byte> DeletedCollection
        {
            get { return deletedCollection; }
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Clears all the registration in the repository context.
        /// </summary>
        /// <remarks>Note that this can only be called after the repository context has successfully committed.</remarks>
        protected void ClearRegistrations()
        {
            newCollection.Clear();
            modifiedCollection.Clear();
            deletedCollection.Clear();
        }
        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">A <see cref="System.Boolean"/> value which indicates whether
        /// the object should be disposed explicitly.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ClearRegistrations();
            }
        }
        #endregion

        #region IRepositoryContext Members

        /// <summary>
        /// Registers a new object to the repository context.
        /// </summary>
        /// <param name="obj">The object to be registered.</param>
        public virtual void RegisterNew<T>(T obj) where T : Entity
        {
            newCollection.AddOrUpdate(obj, byte.MinValue, (o, b) => byte.MinValue);
            Committed = false;
        }

        /// <summary>
        /// Registers a modified object to the repository context.
        /// </summary>
        /// <param name="obj">The object to be registered.</param>
        public virtual void RegisterModified<T>(T obj) where T : Entity
        {
            if (deletedCollection.ContainsKey(obj))
                throw new InvalidOperationException("The object cannot be registered as a modified object since it was marked as deleted.");
            if (!modifiedCollection.ContainsKey(obj) && !(newCollection.ContainsKey(obj)))
                modifiedCollection.AddOrUpdate(obj, byte.MinValue, (o, b) => byte.MinValue);

            Committed = false;
        }

        /// <summary>
        /// Registers a deleted object to the repository context.
        /// </summary>
        /// <param name="obj">The object to be registered.</param>
        public virtual void RegisterDeleted<T>(T obj) where T : Entity
        {
            var @byte = byte.MinValue;
            if (newCollection.ContainsKey(obj))
            {
                newCollection.TryRemove(obj, out @byte);
                return;
            }
            var removedFromModified = modifiedCollection.TryRemove(obj, out @byte);
            var addedToDeleted = false;
            if (!deletedCollection.ContainsKey(obj))
            {
                deletedCollection.AddOrUpdate(obj, byte.MinValue, (o, b) => byte.MinValue);
                addedToDeleted = true;
            }
            committed = !(removedFromModified || addedToDeleted);
        }
        #endregion

        #region IUnitOfWork Members
        public virtual bool DistributedTransactionSupported
        {
            get { return false; }
        }

        public virtual bool Committed
        {
            get { return committed; }
            protected set { committed = value; }
        }

        public abstract void Commit();

        public Task CommitAsync()
        {
            return CommitAsync(CancellationToken.None);
        }

        public abstract Task CommitAsync(CancellationToken cancellationToken);

        public abstract void Rollback();
        #endregion
    }
}

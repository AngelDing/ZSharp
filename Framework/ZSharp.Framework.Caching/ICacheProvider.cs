using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ZSharp.Framework.Caching
{    
    public interface ICacheProvider
    {
        CacheType CacheType { get; }

        T Get<T>(string key);

        void Set(CacheKey key, object value, CachePolicy cachePolicy);

        bool Contains(string key);

        void Remove(string key);

        void RemoveByPattern(string pattern);

        void ClearAll();

        void Expire(CacheTag cacheTag);

        /// <summary>
        /// Returns a wrapped sync lock for the underlying <c>ICache</c> implementation
        /// </summary>
        /// <returns>The disposable sync lock</returns>
        /// <remarks>
        /// This method internally wraps either a <c>ReaderWriterLockSlim</c> or an empty noop action
        /// dependending on the scope of the underlying <c>ICache</c> implementation.
        /// The static (singleton) cache always returns the <c>ReaderWriterLockSlim</c> instance
        /// which is used to sync read/write access to cache items.
        /// This method is useful if you want to modify a cache item's value, thus must lock access
        /// to the cache during the update.
        /// </remarks>
        IDisposable EnterWriteLock();

        IDisposable EnterReadLock();
    }
}

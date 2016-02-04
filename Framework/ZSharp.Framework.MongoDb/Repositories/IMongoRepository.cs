using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZSharp.Framework.Entities;
using ZSharp.Framework.Repositories;

namespace ZSharp.Framework.MongoDb
{
    public interface IMongoRepository<T, TKey> : IRepository<T, TKey>
        where T : IAggregateRoot<TKey>
    {
        Task<T> GetByKeyAsync(TKey id);

        void Update(Expression<Func<T, bool>> query, Dictionary<string, object> columnValues);

        Task<T> FindOneAndUpdateAsync(Expression<Func<T, bool>> query, Dictionary<string, object> columnValues);

        void Update(Expression<Func<T, bool>> query, T entity);

        void RemoveAll();     
    }
}

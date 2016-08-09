using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ZSharp.Framework.Entities;
using ZSharp.Framework.Repositories;

namespace ZSharp.Framework.SqlDb
{
    public interface IEfRepository<T, TKey> : IRepository<T, TKey> where T : IAggregateRoot<TKey>
    {
        void BulkDelete(Expression<Func<T, bool>> predicate);

        void BulkInsert(IEnumerable<T> entities);

        /// <summary>
        /// 实现按需要只更新部分更新
        /// <para>如：Update(u =>u.Id==1,u =>new User{Name="ok"});</para>
        /// </summary>
        /// <param name="predicate">The where.</param>
        /// <param name="funcEntity">The entity.</param>
        void Update(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> funcEntity);
    }
}

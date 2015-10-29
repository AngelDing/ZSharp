using System;
using System.Data.Entity.Core.Objects;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ZSharp.Framework.EfExtensions.Batch
{
    public interface IBatchUpdate
    {
        int Update<T>(ObjectContext objectContext, ObjectQuery<T> query, Expression<Func<T, T>> updateExpression)
            where T : class;

        Task<int> UpdateAsync<T>(ObjectContext objectContext, ObjectQuery<T> query, Expression<Func<T, T>> updateExpression)
            where T : class;
    }
}

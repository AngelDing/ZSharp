using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using ZSharp.Framework.EfExtensions.Batch;
using ZSharp.Framework.Dependency;

namespace ZSharp.Framework.EfExtensions
{
    public static class BatchExtensions
    {
        public static int Delete<T>(this IQueryable<T> source)
            where T : class
        {
            var sourceQuery = source.ToObjectQuery();
            if (sourceQuery == null)
                throw new ArgumentException("The query must be of type ObjectQuery or DbQuery.", "source");

            var objectContext = sourceQuery.Context;
            if (objectContext == null)
                throw new ArgumentException("The ObjectContext for the source query can not be null.", "source");

            var runner = SimpleLocator<EfLocator>.Current.Resolve<IBatchDelete>();
            return runner.Delete(objectContext, sourceQuery);
        }      

        public static Task<int> DeleteAsync<T>(this IQueryable<T> source)
            where T : class
        {
            var sourceQuery = source.ToObjectQuery();
            if (sourceQuery == null)
                throw new ArgumentException("The query must be of type ObjectQuery or DbQuery.", "source");

            var objectContext = sourceQuery.Context;
            if (objectContext == null)
                throw new ArgumentException("The ObjectContext for the source query can not be null.", "source");

            var runner = SimpleLocator<EfLocator>.Current.Resolve<IBatchDelete>();
            return runner.DeleteAsync(objectContext, sourceQuery);
        }      

        public static int Update<T>(this IQueryable<T> source,
            Expression<Func<T, T>> updateExpression)
            where T : class
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (updateExpression == null)
                throw new ArgumentNullException("updateExpression");

            var sourceQuery = source.ToObjectQuery();
            if (sourceQuery == null)
                throw new ArgumentException("The query must be of type ObjectQuery or DbQuery.", "source");

            var objectContext = sourceQuery.Context;
            if (objectContext == null)
                throw new ArgumentException("The ObjectContext for the query can not be null.", "source");

            var runner = SimpleLocator<EfLocator>.Current.Resolve<IBatchUpdate>();
            return runner.Update(objectContext, sourceQuery, updateExpression);
        }

        public static Task<int> UpdateAsync<T>(this IQueryable<T> source,
            Expression<Func<T, T>> updateExpression)
            where T : class
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (updateExpression == null)
                throw new ArgumentNullException("updateExpression");

            var sourceQuery = source.ToObjectQuery();
            if (sourceQuery == null)
                throw new ArgumentException("The query must be of type ObjectQuery or DbQuery.", "source");

            var objectContext = sourceQuery.Context;
            if (objectContext == null)
                throw new ArgumentException("The ObjectContext for the query can not be null.", "source");

            var runner = SimpleLocator<EfLocator>.Current.Resolve<IBatchUpdate>();
            return runner.UpdateAsync(objectContext, sourceQuery, updateExpression);
        }

        public static void BulkInsert<TEntity>(this DbContext dbContext,
            IEnumerable<TEntity> entities, int batchSize = 5000)
            where TEntity : class
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext");

            var runner = SimpleLocator<EfLocator>.Current.Resolve<IBatchInsert>();
            runner.Insert(dbContext, entities, batchSize);
        }
    }
}

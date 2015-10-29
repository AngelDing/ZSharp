using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using ZSharp.Framework.Results;

namespace ZSharp.Framework.EfExtensions
{
    public static class EntityFrameworkHelper
    {
        public static IQueryable<T> Include<T>(this IDbSet<T> dbSet, Expression<Func<T, dynamic>> exp)
            where T : class
        {
            return dbSet.Include(GetStrForInclude(exp));
        }

        public static IQueryable<T> Include<T>(this IQueryable<T> query, Expression<Func<T, dynamic>> exp)
            where T : class
        {
            return query.Include(GetStrForInclude(exp));
        }

        private static string GetStrForInclude<T>(Expression<Func<T,dynamic>> exp)
            where T : class
        {
            string str = string.Empty;
            Expression expression = exp.Body;
            if (!(expression is MemberExpression))
            {
                throw new ArgumentException("Must be 'MemberExpression'.");
            }
            while (expression is MemberExpression || expression is MethodCallExpression)
            {
                var memberExp = expression as MemberExpression;
                if (memberExp != null)
                {
                    str = string.Concat(memberExp.Member.Name, ".", str);
                    expression = memberExp.Expression;
                }
                else
                {
                    var callExp = expression as MethodCallExpression;
                    if (callExp == null || callExp.Arguments == null || callExp.Arguments.Count == 0)
                    {
                        throw new ArgumentException("Not a right format expression.");
                    }
                    expression = callExp.Arguments[0];
                }
            }

            return str.TrimEnd('.');
        }

        public static TSource Find<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
          where TSource : class
        {
            return source.Where(predicate).FirstOrDefault();
        }

        public static TSource Find<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
         where TSource : class
        {
            return source.Where(predicate).FirstOrDefault();
        }
        public static PagingResult<T> Paging<T>(this IQueryable<T> source, int pageIndex = 1, int pageSize = 20)
            where T : class
        {
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }

            var skipCount = (pageIndex - 1) * pageSize;
            var totalCount = 0;
            IEnumerable<T> entities;

            var objQuery = source.ToObjectQuery();
            if (objQuery == null)
            {
                //當已經獲取到Entities，然後轉為AsQueryable()時，直接分頁即可，不需要再次從數據庫獲取；
                //此種情況多為從服務獲取數據列表后，然後再分頁；
                totalCount = source.Count();
                entities = source.ToList().Skip(skipCount).Take(pageSize);
            }
            else
            {
                var resoultCount = source.FutureCount(objQuery);
                entities = source.Skip(skipCount).Take(pageSize).Future();
                totalCount = resoultCount.Value;               
            }
            return new PagingResult<T>(totalCount, pageIndex, pageSize, entities);
        }

        public static TEntity Insert<TEntity>(this DbContext context, TEntity entity)
            where TEntity : class
        {
            context.Set<TEntity>().Add(entity);
            context.SaveChanges();
            return entity;
        }       

        public static TEntity Update<TEntity>(this DbContext context, TEntity entity)
            where TEntity : class
        {
            context.Entry<TEntity>(entity).State = EntityState.Modified;
            context.SaveChanges();
            return entity;
        }

        public static void Delete<TEntity>(this DbContext context, TEntity entity)
            where TEntity : class
        {
            context.Set<TEntity>().Remove(entity);
            context.Entry<TEntity>(entity).State = EntityState.Deleted;
            context.SaveChanges();
        }

        public static void ExecuteNonQuery(this Database database, DbCommand cmd)
        {
            if (null == database || database.Connection == null)
                throw new ArgumentNullException("database");
            if (null == cmd)
                throw new ArgumentNullException("cmd");
            if (cmd.Connection == null)
                cmd.Connection = new SqlConnection(database.Connection.ConnectionString);
            try
            {
                Monitor.Enter(cmd.Connection);
                if (cmd.Connection.State != System.Data.ConnectionState.Open)
                    cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
                Monitor.Exit(cmd.Connection);
            }
        }
    }
}
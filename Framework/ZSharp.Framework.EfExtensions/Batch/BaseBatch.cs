using System;
using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Linq.Dynamic;
using System.Text;
using ZSharp.Framework.EfExtensions.Mapping;
using ZSharp.Framework.EfExtensions.Reflection;

namespace ZSharp.Framework.EfExtensions.Batch
{
    public class BaseBatch
    {
        public EntityMap GetEntityMap<T>(ObjectContext objectContext)
            where T : class
        {
            var entityMap = objectContext.GetEntityMap<T>(typeof(T));
            return entityMap;
        }

        public Tuple<DbConnection, DbTransaction> GetStore(ObjectContext objectContext)
        {
            // TODO, re-eval if this is needed

            DbConnection dbConnection = objectContext.Connection;
            var entityConnection = dbConnection as EntityConnection;

            // by-pass entity connection
            if (entityConnection == null)
                return new Tuple<DbConnection, DbTransaction>(dbConnection, null);

            DbConnection connection = entityConnection.StoreConnection;

            // get internal transaction
            dynamic connectionProxy = new DynamicProxy(entityConnection);
            dynamic entityTransaction = connectionProxy.CurrentTransaction;
            if (entityTransaction == null)
                return new Tuple<DbConnection, DbTransaction>(connection, null);

            DbTransaction transaction = entityTransaction.StoreTransaction;
            return new Tuple<DbConnection, DbTransaction>(connection, transaction);
        }

        public string GetSelectSql<T>(ObjectQuery<T> query, EntityMap entityMap, DbCommand command)
            where T : class
        {
            // changing query to only select keys
            var selector = new StringBuilder(50);
            selector.Append("new(");
            foreach (var propertyMap in entityMap.KeyMaps)
            {
                if (selector.Length > 4)
                    selector.Append((", "));

                selector.Append(propertyMap.PropertyName);
            }
            selector.Append(")");

            var selectQuery = DynamicQueryable.Select(query, selector.ToString());
            var objectQuery = selectQuery as ObjectQuery;

            if (objectQuery == null)
                throw new ArgumentException("The query must be of type ObjectQuery.", "query");

            string innerJoinSql = objectQuery.ToTraceString();

            // create parameters
            foreach (var objectParameter in objectQuery.Parameters)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = objectParameter.Name;
                parameter.Value = objectParameter.Value ?? DBNull.Value;

                command.Parameters.Add(parameter);
            }

            return innerJoinSql;
        }
    }
}
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZSharp.Framework.EfExtensions.Batch
{
    public class BatchDeleteProvider : BaseBatch, IBatchDelete
    {
        private const string StopInvalidQueryString = 
            @"SELECT1AS[C1],CAST(NULLASint)AS[C2]FROM(SELECT1ASX)AS[SingleRowTable1]WHERE1=0";

        public int Delete<T>(ObjectContext objectContext, ObjectQuery<T> query) 
            where T : class
        {
            return InternalDelete(objectContext, query, false).Result;
        }

        public Task<int> DeleteAsync<T>(ObjectContext objectContext, ObjectQuery<T> query)
            where T : class
        {
            return InternalDelete(objectContext, query, true);
        }

        private async Task<int> InternalDelete<T>(ObjectContext objectContext, ObjectQuery<T> query, bool async = false)
            where T : class
        {
            DbConnection deleteConnection = null;
            DbTransaction deleteTransaction = null;
            DbCommand deleteCommand = null;
            bool ownConnection = false;
            bool ownTransaction = false;

            try
            {
                // get store connection and transaction
                var store = GetStore(objectContext);
                deleteConnection = store.Item1;
                deleteTransaction = store.Item2;

                if (deleteConnection.State != ConnectionState.Open)
                {
                    deleteConnection.Open();
                    ownConnection = true;
                }

                if (deleteTransaction == null)
                {
                    deleteTransaction = deleteConnection.BeginTransaction();
                    ownTransaction = true;
                }


                deleteCommand = deleteConnection.CreateCommand();
                deleteCommand.Transaction = deleteTransaction;
                if (objectContext.CommandTimeout.HasValue)
                    deleteCommand.CommandTimeout = objectContext.CommandTimeout.Value;

                var entityMap = GetEntityMap<T>(objectContext);
                var innerSelect = GetSelectSql(query, entityMap, deleteCommand);
                //有短路條件產生,直接返回0
                var checkingSelector =  Regex.Replace(innerSelect,@"\s","");
                if (Regex.IsMatch(checkingSelector, @"\[SingleRowTable1\]WHERE1=0"))
                {
                    return 0;
                }
                var sqlBuilder = new StringBuilder(innerSelect.Length * 2);

                sqlBuilder.Append("DELETE ");
                sqlBuilder.Append(entityMap.TableFullName);
                sqlBuilder.AppendLine();

                sqlBuilder.AppendFormat("FROM {0} AS j0 INNER JOIN (", entityMap.TableFullName);
                sqlBuilder.AppendLine();
                sqlBuilder.AppendLine(innerSelect);
                sqlBuilder.Append(") AS j1 ON (");

                bool wroteKey = false;
                foreach (var keyMap in entityMap.KeyMaps)
                {
                    if (wroteKey)
                        sqlBuilder.Append(" AND ");

                    sqlBuilder.AppendFormat("j0.[{0}] = j1.[{0}]", keyMap.ColumnName);
                    wroteKey = true;
                }
                sqlBuilder.Append(")");

                deleteCommand.CommandText = sqlBuilder.ToString();

                int result = async
                    ? await deleteCommand.ExecuteNonQueryAsync()
                    : deleteCommand.ExecuteNonQuery();

                // only commit if created transaction
                if (ownTransaction)
                    deleteTransaction.Commit();

                return result;
            }
            finally
            {
                if (deleteCommand != null)
                    deleteCommand.Dispose();

                if (deleteTransaction != null && ownTransaction)
                    deleteTransaction.Dispose();

                if (deleteConnection != null && ownConnection)
                    deleteConnection.Close();
            }
        }       
    }
}
using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZSharp.Framework.EfExtensions.Batch
{
    public class BatchUpdateProvider : BaseBatch, IBatchUpdate
    {
        public int Update<T>(
            ObjectContext objectContext, 
            ObjectQuery<T> query, 
            Expression<Func<T, T>> updateExpression)
            where T : class
        {
            return InternalUpdate(objectContext, query, updateExpression, false).Result;
        }

        public Task<int> UpdateAsync<T>(
            ObjectContext objectContext,
            ObjectQuery<T> query,
            Expression<Func<T, T>> updateExpression)
            where T : class
        {
            return InternalUpdate(objectContext, query, updateExpression, true);
        }

        private async Task<int> InternalUpdate<T>(
            ObjectContext objectContext, 
            ObjectQuery<T> query, 
            Expression<Func<T, T>> updateExpression,
            bool async = false)
            where T : class
        {
            DbConnection updateConnection = null;
            DbTransaction updateTransaction = null;
            DbCommand updateCommand = null;
            bool ownConnection = false;
            bool ownTransaction = false;

            try
            {
                // get store connection and transaction
                var store = GetStore(objectContext);
                updateConnection = store.Item1;
                updateTransaction = store.Item2;

                if (updateConnection.State != ConnectionState.Open)
                {
                    updateConnection.Open();
                    ownConnection = true;
                }

                // use existing transaction or create new
                if (updateTransaction == null)
                {
                    updateTransaction = updateConnection.BeginTransaction();
                    ownTransaction = true;
                }

                updateCommand = updateConnection.CreateCommand();
                updateCommand.Transaction = updateTransaction;
                if (objectContext.CommandTimeout.HasValue)
                    updateCommand.CommandTimeout = objectContext.CommandTimeout.Value;

                var entityMap = GetEntityMap<T>(objectContext);
                var innerSelect = GetSelectSql(query, entityMap, updateCommand);
                var sqlBuilder = new StringBuilder(innerSelect.Length * 2);

                sqlBuilder.Append("UPDATE ");
                sqlBuilder.Append(entityMap.TableFullName);
                sqlBuilder.AppendLine(" SET ");

                var memberInitExpression = updateExpression.Body as MemberInitExpression;
                if (memberInitExpression == null)
                    throw new ArgumentException("The update expression must be of type MemberInitExpression.", "updateExpression");

                int nameCount = 0;
                bool wroteSet = false;
                foreach (MemberBinding binding in memberInitExpression.Bindings)
                {
                    if (wroteSet)
                        sqlBuilder.AppendLine(", ");

                    string propertyName = binding.Member.Name;
                    string columnName = entityMap.PropertyMaps
                        .Where(p => p.PropertyName == propertyName)
                        .Select(p => p.ColumnName)
                        .FirstOrDefault();


                    var memberAssignment = binding as MemberAssignment;
                    if (memberAssignment == null)
                        throw new ArgumentException("The update expression MemberBinding must only by type MemberAssignment.", "updateExpression");

                    Expression memberExpression = memberAssignment.Expression;

                    ParameterExpression parameterExpression = null;
                    memberExpression.Visit((ParameterExpression p) =>
                    {
                        if (p.Type == entityMap.EntityType)
                            parameterExpression = p;

                        return p;
                    });


                    if (parameterExpression == null)
                    {
                        object value;

                        if (memberExpression.NodeType == ExpressionType.Constant)
                        {
                            var constantExpression = memberExpression as ConstantExpression;
                            if (constantExpression == null)
                                throw new ArgumentException(
                                    "The MemberAssignment expression is not a ConstantExpression.", "updateExpression");

                            value = constantExpression.Value;
                        }
                        else
                        {
                            LambdaExpression lambda = Expression.Lambda(memberExpression, null);
                            value = lambda.Compile().DynamicInvoke();
                        }

                        if (value != null)
                        {
                            string parameterName = "p__update__" + nameCount++;
                            var parameter = updateCommand.CreateParameter();
                            parameter.ParameterName = parameterName;
                            parameter.Value = value;
                            updateCommand.Parameters.Add(parameter);

                            sqlBuilder.AppendFormat("[{0}] = @{1}", columnName, parameterName);
                        }
                        else
                        {
                            sqlBuilder.AppendFormat("[{0}] = NULL", columnName);
                        }
                    }
                    else
                    {
                        // create clean objectset to build query from
                        var objectSet = objectContext.CreateObjectSet<T>();

                        Type[] typeArguments = new[] { entityMap.EntityType, memberExpression.Type };

                        ConstantExpression constantExpression = Expression.Constant(objectSet);
                        LambdaExpression lambdaExpression = Expression.Lambda(memberExpression, parameterExpression);

                        MethodCallExpression selectExpression = Expression.Call(
                            typeof(Queryable),
                            "Select",
                            typeArguments,
                            constantExpression,
                            lambdaExpression);

                        // create query from expression
                        var selectQuery = objectSet.CreateQuery(selectExpression, entityMap.EntityType);
                        string sql = selectQuery.ToTraceString();

                        // parse select part of sql to use as update
                        string regex = @"SELECT\s*\r\n\s*(?<ColumnValue>.+)?\s*AS\s*(?<ColumnAlias>\[\w+\])\r\n\s*FROM\s*(?<TableName>\[\w+\]\.\[\w+\]|\[\w+\])\s*AS\s*(?<TableAlias>\[\w+\])";
                        Match match = Regex.Match(sql, regex);
                        if (!match.Success)
                            throw new ArgumentException("The MemberAssignment expression could not be processed.", "updateExpression");

                        string value = match.Groups["ColumnValue"].Value;
                        string alias = match.Groups["TableAlias"].Value;

                        value = value.Replace(alias + ".", "");

                        foreach (ObjectParameter objectParameter in selectQuery.Parameters)
                        {
                            string parameterName = "p__update__" + nameCount++;

                            var parameter = updateCommand.CreateParameter();
                            parameter.ParameterName = parameterName;
                            parameter.Value = objectParameter.Value ?? DBNull.Value;
                            updateCommand.Parameters.Add(parameter);

                            value = value.Replace(objectParameter.Name, parameterName);
                        }
                        sqlBuilder.AppendFormat("[{0}] = {1}", columnName, value);
                    }
                    wroteSet = true;
                }

                sqlBuilder.AppendLine(" ");
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

                updateCommand.CommandText = sqlBuilder.ToString();

                int result = async
                    ? await updateCommand.ExecuteNonQueryAsync()
                    : updateCommand.ExecuteNonQuery();

                // only commit if created transaction
                if (ownTransaction)
                    updateTransaction.Commit();

                return result;
            }
            finally
            {
                if (updateCommand != null)
                    updateCommand.Dispose();
                if (updateTransaction != null && ownTransaction)
                    updateTransaction.Dispose();
                if (updateConnection != null && ownConnection)
                    updateConnection.Close();
            }
        }               
    }
}
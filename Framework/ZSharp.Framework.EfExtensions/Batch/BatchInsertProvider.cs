using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using ZSharp.Framework.EfExtensions.Batch;
using ZSharp.Framework.EfExtensions.Mapping;

namespace ZSharp.Framework.EfExtensions.Batch
{
    internal class BatchInsertProvider : IBatchInsert
    {
        public void Insert<T>(DbContext dbContext, IEnumerable<T> entities, int batchSize)
            where T : class
        {           
            var connStr = dbContext.Database.Connection.ConnectionString;
            var entiyMap = dbContext.GetEntityMap<T>(typeof(T));

            using (var dbConnection = new SqlConnection(connStr))
            {
                dbConnection.Open();

                using (var transaction = dbConnection.BeginTransaction())
                {
                    try
                    {
                        Insert(entities, transaction, entiyMap, batchSize);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (transaction.Connection != null)
                        {
                            transaction.Rollback();
                        }
                        throw ex;
                    }
                }
            }
        }

        private void Insert<T>(IEnumerable<T> entities, SqlTransaction transaction,
            EntityMap entiyMap, int batchSize)
        {
            var options = SqlBulkCopyOptions.Default;
            using (var reader = new MappedDataReader<T>(entities, entiyMap))
            {
                using (var sqlBulkCopy = new SqlBulkCopy(transaction.Connection, options, transaction))
                {
                    sqlBulkCopy.BatchSize = batchSize;
                    sqlBulkCopy.DestinationTableName = entiyMap.TableFullName ;
                    foreach (var kvp in reader.Cols)
                    {
                        if (kvp.Value.IsIdentity)
                        {
                            continue;
                        }
                        sqlBulkCopy.ColumnMappings.Add(kvp.Value.ColumnName, kvp.Value.ColumnName);
                    }

                    sqlBulkCopy.WriteToServer(reader);
                }
            }
        }

        //private void Insert<T>(IEnumerable<T> entities, SqlTransaction transaction, 
        //    EntityMap entiyMap, int batchSize)
        //{
        //    var options = SqlBulkCopyOptions.Default;

        //    using (DataTable dataTable = new MappedDataTable().CreateDataTable(entiyMap, entities))
        //    {
        //        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(transaction.Connection, options, transaction))
        //        {
        //            sqlBulkCopy.BatchSize = batchSize;
        //            sqlBulkCopy.DestinationTableName = dataTable.TableName;
        //            sqlBulkCopy.WriteToServer(dataTable);
        //        }
        //    }
        //}        
    }
}

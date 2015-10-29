using ZSharp.Framework.EfExtensions.Mapping;
using System;
using System.Collections.Generic;
using System.Data;

namespace ZSharp.Framework.EfExtensions.Batch
{
    public class MappedDataTable<T>
    {
        private EntityMap entiyMap;
        private IEnumerable<T> entities;
        public MappedDataTable(EntityMap entiyMap, IEnumerable<T> entities)
        {
            this.entiyMap = entiyMap;
            this.entities = entities;
        }
        public DataTable CreateDataTable()
        {
            var dataTable = BuildDataTable();

            foreach (var entity in entities)
            {
                DataRow row = dataTable.NewRow();

                foreach (var column in entiyMap.PropertyMaps)
                {
                    var @value = entity.GetPropertyValue(column.PropertyName);

                    if (column.IsIdentity) continue;

                    if (@value == null)
                    {
                        row[column.ColumnName] = DBNull.Value;
                    }
                    else
                    {
                        row[column.ColumnName] = @value;
                    }
                }

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        private DataTable BuildDataTable()
        {
            var entityType = typeof(T);
            string tableName = entiyMap.TableFullName;

            var dataTable = new DataTable(tableName);
            var primaryKeys = new List<DataColumn>();

            foreach (var columnMapping in entiyMap.PropertyMaps)
            {
                var propertyInfo = entityType.GetProperty(columnMapping.PropertyName, '.');
                columnMapping.Type = propertyInfo.PropertyType;

                var dataColumn = new DataColumn(columnMapping.ColumnName);

                Type dataType;
                if (propertyInfo.PropertyType.IsNullable(out dataType))
                {
                    dataColumn.DataType = dataType;
                    dataColumn.AllowDBNull = true;
                }
                else
                {
                    dataColumn.DataType = propertyInfo.PropertyType;
                    dataColumn.AllowDBNull = columnMapping.Nullable;
                }

                if (columnMapping.IsIdentity)
                {
                    dataColumn.Unique = true;
                    if (propertyInfo.PropertyType == typeof(int)
                      || propertyInfo.PropertyType == typeof(long))
                    {
                        dataColumn.AutoIncrement = true;
                    }
                    else continue;
                }
                else
                {
                    dataColumn.DefaultValue = columnMapping.DefaultValue;
                }

                if (propertyInfo.PropertyType == typeof(string))
                {
                    dataColumn.MaxLength = columnMapping.MaxLength;
                }

                if (columnMapping.IsPk)
                {
                    primaryKeys.Add(dataColumn);
                }

                dataTable.Columns.Add(dataColumn);
            }

            dataTable.PrimaryKey = primaryKeys.ToArray();

            return dataTable;
        }
    }
}

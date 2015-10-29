using System;

namespace ZSharp.Framework.EfExtensions.Mapping
{
    public class PropertyMap
    {
        public PropertyMap(string property, string columnName)
        {
            PropertyName = property;
            ColumnName = columnName;
        }

        public string PropertyName { get; private set; }

        public string ColumnName { get; private set; }

        public Type Type { get; set; }

        public bool IsPk { get; internal set; }

        public bool IsFk { get; internal set; }

        public bool IsNavigationProperty { get; internal set; }

        public bool IsDiscriminator { get; internal set; }

        public bool Nullable { get; internal set; }

        public object DefaultValue { get; internal set; }

        public bool IsIdentity { get; internal set; }

        public bool Computed { get; internal set; }

        public int MaxLength { get; internal set; }
    }
}
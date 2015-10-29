using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;

namespace ZSharp.Framework.EfExtensions.Mapping
{
    public class EntityMap
    {
        public EntityMap(Type entityType)
        {
            EntityType = entityType;
            PropertyMaps = new List<PropertyMap>();
        }

        public EntitySet ModelSet { get; set; }

        public EntitySet StoreSet { get; set; }

        public EntityType ModelType { get; set; }

        public EntityType StoreType { get; set; }

        public string TableFullName { get; set; }

        public string SchemaName { get; set; }

        public string TableName { get; set; }

        public Type EntityType { get; private set; }

        public List<PropertyMap> PropertyMaps { get; private set; }

        public List<PropertyMap> KeyMaps
        {
            get
            {
                return PropertyMaps.Where(p => p.IsPk == true).ToList();
            }
        }

        public PropertyMap AddProperty(string property, string columnName)
        {
            var map = new PropertyMap(property, columnName);
            PropertyMaps.Add(map);

            return map;
        }
    }
}
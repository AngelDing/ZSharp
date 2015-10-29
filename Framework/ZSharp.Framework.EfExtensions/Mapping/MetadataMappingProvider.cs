using System;
using System.Collections.Concurrent;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;

namespace ZSharp.Framework.EfExtensions.Mapping
{
    public class MetadataMappingProvider : IMappingProvider
    {
        private static ConcurrentDictionary<Type, EntityMap> entityMapCache =
            new ConcurrentDictionary<Type, EntityMap>();

        public EntityMap GetEntityMap(Type type, DbContext dbContext)
        {
            var objectContextAdapter = dbContext as IObjectContextAdapter;
            var objectContext = objectContextAdapter.ObjectContext;

            var key = type.FullName;
            return GetEntityMap(type, objectContext);
        }


        public EntityMap GetEntityMap(Type type, ObjectContext objectContext)
        {
            return entityMapCache.GetOrAdd(type, key => RealGetEntityMap(key, objectContext));
        }

        private static EntityMap RealGetEntityMap(Type type, ObjectContext objectContext)
        {
            var entityMap = new EntityMap(type);
            var metadata = objectContext.MetadataWorkspace;

            // Get the part of the model that contains info about the actual CLR types
            var objectItemCollection = ((ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace));

            // Get the entity type from the model that maps to the CLR type
            var entityType = metadata
                    .GetItems<EntityType>(DataSpace.OSpace)
                    .First(e => objectItemCollection.GetClrType(e) == type);

            // Get the entity set that uses this entity type
            var entitySet = metadata
                .GetItems<EntityContainer>(DataSpace.CSpace)
                .First()
                .EntitySets
                .First(s => s.ElementType.Name == entityType.Name);

            // Find the mapping between conceptual and storage model for this entity set
            var mapping = metadata.GetItems<EntityContainerMapping>(DataSpace.CSSpace)
                    .First()
                    .EntitySetMappings
                    .First(s => s.EntitySet == entitySet);

            // Find the storage entity set (table) that the entity is mapped
            var etMap = mapping.EntityTypeMappings.FirstOrDefault(a => a.IsHierarchyMapping);
            etMap = etMap ?? mapping.EntityTypeMappings.First();
            var mappingFragment = etMap.Fragments.First();

            entityMap.ModelType = entityType;
            entityMap.ModelSet = entitySet;
            entityMap.StoreSet = mappingFragment.StoreEntitySet;
            entityMap.StoreType = mappingFragment.StoreEntitySet.ElementType;

            // set table
            SetTableName(entityMap);
            // set properties
            SetProperties(entityMap, mappingFragment);
            // set keys
            SetKeys(entityMap);

            return entityMap;
        }

        private static void SetKeys(EntityMap entityMap)
        {
            var modelType = entityMap.ModelType;
            foreach (var edmMember in modelType.KeyMembers)
            {
                var pMap = entityMap.PropertyMaps
                    .FirstOrDefault(p => p.PropertyName == edmMember.Name);
                if (pMap != null)
                {
                    pMap.IsPk = true;
                }
            }
        }        

        private static void SetProperties(EntityMap entityMap, MappingFragment mappingFragment)
        {
            foreach (var propertyMapping in mappingFragment.PropertyMappings)
            {
                var pName = propertyMapping.Property.Name;               

                var scalarPropertyMapping = propertyMapping as ScalarPropertyMapping;
                if (scalarPropertyMapping != null)
                {
                    var cName = scalarPropertyMapping.Column.Name;
                    var pMap = entityMap.AddProperty(pName, cName);
                    BuildPropertyMapping(scalarPropertyMapping.Column, pMap);
                }              
                //// TODO support complex mapping
                //var complexPropertyMapping = propertyMapping as ComplexPropertyMapping;
            }
        }

        private static void BuildPropertyMapping(EdmProperty edm, PropertyMap pMap)
        {
            foreach (var facet in edm.TypeUsage.Facets)
            {
                switch (facet.Name)
                {
                    case "Nullable":
                        pMap.Nullable = (bool)facet.Value;
                        break;
                    case "DefaultValue":
                        pMap.DefaultValue = facet.Value;
                        break;
                    case "StoreGeneratedPattern":
                        pMap.IsIdentity = (StoreGeneratedPattern)facet.Value == StoreGeneratedPattern.Identity;
                        pMap.Computed = (StoreGeneratedPattern)facet.Value == StoreGeneratedPattern.Computed;
                        break;
                    case "MaxLength":
                        pMap.MaxLength = (int)facet.Value;
                        break;
                }
            }
        }

        private static void SetTableName(EntityMap entityMap)
        {          
            var storeSet = entityMap.StoreSet;

            var tableName = storeSet.Name;
            var schemaName = storeSet.Schema;
            if (string.IsNullOrEmpty(tableName))
            {
                tableName = (string)storeSet.MetadataProperties["Table"].Value;
            }
            if (string.IsNullOrEmpty(schemaName))
            { 
                schemaName = (string)storeSet.MetadataProperties["Schema"].Value;
            }
            entityMap.TableName = tableName;
            entityMap.SchemaName = schemaName;

            var builder = new StringBuilder(100);
            builder.Append(QuoteIdentifier(schemaName));
            builder.Append(".");
            builder.Append(QuoteIdentifier(tableName));
            entityMap.TableFullName = builder.ToString();
        }

        private static string QuoteIdentifier(string name)
        {
            return ("[" + name.Replace("]", "]]") + "]");
        }
    }
}

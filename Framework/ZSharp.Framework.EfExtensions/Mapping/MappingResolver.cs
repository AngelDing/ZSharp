using ZSharp.Framework.Dependency;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;

namespace ZSharp.Framework.EfExtensions.Mapping
{
    public static class MappingResolver
    {
        public static EntityMap GetEntityMap<TEntity>(this DbContext dbContext, Type type)
        {
            var provider = SimpleLocator<EfLocator>.Current.Resolve<IMappingProvider>();
            return provider.GetEntityMap(type, dbContext);
        }

        public static EntityMap GetEntityMap<TEntity>(this ObjectContext objectContext, Type type)
        {
            var provider = SimpleLocator<EfLocator>.Current.Resolve<IMappingProvider>();
            return provider.GetEntityMap(type, objectContext);
        }
    }
}

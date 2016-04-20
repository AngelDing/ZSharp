using ZSharp.Framework.Dependency;
using ZSharp.Framework.EfExtensions.Batch;
using ZSharp.Framework.EfExtensions.Future;
using ZSharp.Framework.EfExtensions.Mapping;

namespace ZSharp.Framework.EfExtensions
{
    public class EfLocator : SimpleLocator
    {
        public override void RegisterDefaults(IContainer container)
        {
            var transient = DependencyLifecycle.Transient;
            container.Register<IMappingProvider>(p => new MetadataMappingProvider());
            container.Register<IBatchInsert>(p => new BatchInsertProvider(), transient);
            container.Register<IBatchUpdate>(p => new BatchUpdateProvider(), transient);
            container.Register<IBatchDelete>(p => new BatchDeleteProvider(), transient);
            container.Register<IFutureRunner>(p => new FutureRunner(), transient);
        }
    }
}

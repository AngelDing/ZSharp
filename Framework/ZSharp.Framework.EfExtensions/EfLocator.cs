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
            container.Register<IMappingProvider>(() => new MetadataMappingProvider());
            container.Register<IBatchInsert>(() => new BatchInsertProvider());
            container.Register<IBatchUpdate>(() => new BatchUpdateProvider());
            container.Register<IBatchDelete>(() => new BatchDeleteProvider());           
            container.Register<IFutureRunner>(() => new FutureRunner());
        }
    }
}

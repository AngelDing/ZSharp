using System.Data.Entity.Core.Objects;
using System.Threading.Tasks;

namespace ZSharp.Framework.EfExtensions.Batch
{
    public interface IBatchDelete
    {
        int Delete<T>(ObjectContext objectContext, ObjectQuery<T> query)
            where T : class;

        Task<int> DeleteAsync<T>(ObjectContext objectContext, ObjectQuery<T> query)
            where T : class;
    }
}

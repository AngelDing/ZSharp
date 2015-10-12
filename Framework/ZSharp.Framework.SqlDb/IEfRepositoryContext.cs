using System.Data.Entity;
using ZSharp.Framework.Repositories;

namespace ZSharp.Framework.SqlDb
{
    public interface IEfRepositoryContext : IRepositoryContext
    {
        DbContext DbContext { get; }
    }
}

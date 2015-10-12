using System;
using ZSharp.Framework.Entities;

namespace ZSharp.Framework.Repositories
{
    public interface IRepositoryContext : IUnitOfWork, IDisposable
    {
        void RegisterNew<T>(T obj) where T : BaseEntity;

        void RegisterModified<T>(T obj) where T : BaseEntity;

        void RegisterDeleted<T>(T obj) where T : BaseEntity;
    }
}

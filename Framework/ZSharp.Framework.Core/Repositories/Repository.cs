using ZSharp.Framework.Specifications;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ZSharp.Framework.Entities;

namespace ZSharp.Framework.Repositories
{
    public abstract class Repository<T, TKey> : IRepository<T, TKey>
        where T : IAggregateRoot<TKey>
    {
        private readonly IRepositoryContext repoContext;

        public Repository(IRepositoryContext context)
        {
            this.repoContext = context;
        }

        public IRepositoryContext RepoContext
        {
            get { return repoContext; }
        }

        public virtual void Dispose()
        {
            repoContext.Dispose();
        }

        public abstract void Insert(T entity);

        public abstract void Insert(IEnumerable<T> entities);

        public abstract void Update(T entity);

        public abstract void Update(IEnumerable<T> entities);

        public abstract void Delete(TKey id);

        public abstract void Delete(T entity);

        public abstract void Delete(Expression<Func<T, bool>> predicate);

        public abstract bool Exists(Expression<Func<T, bool>> predicate);

        public abstract T GetByKey(TKey key);

        public abstract IEnumerable<T> GetAll();

        public abstract IEnumerable<T> GetBy(Expression<Func<T, bool>> predicate);

        public abstract IEnumerable<T> GetBy(ISpecification<T> spec);
    }
}

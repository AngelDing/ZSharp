using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Data.Entity;
using ZSharp.Framework.Entities;
using ZSharp.Framework.Specifications;
using ZSharp.Framework.Repositories;
using ZSharp.Framework.EfExtensions;

namespace ZSharp.Framework.SqlDb
{
    public class EfRepository<T, Tkey> : Repository<T, Tkey>, IEfRepository<T, Tkey>
        where T : Entity<Tkey>, IAggregateRoot<Tkey>
    {
        private readonly IEfRepositoryContext efContext;
        private readonly DbContext db;
        private IDbSet<T> dbSet;

        public EfRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEfRepositoryContext)
            {
                efContext = context as IEfRepositoryContext;
                db = efContext.DbContext;
            }          
        }

        #region IRepository

        public override void Insert(T entity)
        {
            efContext.RegisterNew(entity);
        }

        public override void Insert(IEnumerable<T> entities)
        {
            foreach (var e in entities)
            {
                efContext.RegisterNew(e);
            }
        }

        public override void Update(T entity)
        {
            if (entity.ObjectState != ObjectStateType.PartialModified)
            {
                entity.ObjectState = ObjectStateType.Modified;
            }
            efContext.RegisterModified(entity);
            efContext.DbContext.ApplyChanges(new List<T> { entity });
        }

        public override void Update(IEnumerable<T> entities)
        {
            foreach (var e in entities)
            {
                if (e.ObjectState != ObjectStateType.PartialModified)
                {
                    e.ObjectState = ObjectStateType.Modified;
                }
                efContext.RegisterModified(e);
            }
            efContext.DbContext.ApplyChanges(entities);
        }

        public override void Delete(Tkey key)
        {
            var deleteEntity = GetByKey(key);
            Delete(deleteEntity);
        }

        public override void Delete(T entity)
        {
            if (entity is ISoftDeletable)
            {
                entity.NeedUpdateList.Add(nameof(ISoftDeletable.IsDeleted), true);
                Update(entity);
            }
            else
            {
                efContext.RegisterDeleted(entity);
            }            
        }

        public override void Delete(Expression<Func<T, bool>> predicate)
        {
            var deleteList = GetBy(predicate);
            foreach (var e in deleteList)
            {
                Delete(e);
            }
        }

        public override bool Exists(Expression<Func<T, bool>> predicate)
        {
            var count = DbSet.Count(predicate);
            return count != 0;
        }

        #endregion

        #region IEfRepository
        public void BulkDelete(Expression<Func<T, bool>> predicate)
        {
            var queray = DbSet.AsNoTracking().Where(predicate);
            queray.Delete();
        }

        public void BulkInsert(IEnumerable<T> entities)
        {
            db.BulkInsert(entities);
        }

        public void Update(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> funcEntity)
        {
            DbSet.Where(predicate).Update(funcEntity);
        }

        #endregion

        #region IReadOnlyRepository

        public override T GetByKey(Tkey key)
        {
            return DbSet.Find(key);//.FirstOrDefault(p => (object)p.Id == (object)key);
        }

        public override IEnumerable<T> GetAll()
        {
            return DbSet.AsNoTracking().ToList();
        }

        public override IEnumerable<T> GetBy(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate).AsNoTracking().ToList();
        }

        public override IEnumerable<T> GetBy(ISpecification<T> spec)
        {
            return DbSet.Where(spec.SatisfiedBy()).AsNoTracking().ToList();
        }

        public override T Single(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Single(predicate);
        }

        public override T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        public override int Count(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Count(predicate);
        }

        public override long LongCount(Expression<Func<T, bool>> predicate)
        {
            return DbSet.LongCount(predicate);
        }

        #endregion     

        private DbSet<T> DbSet
        {
            get
            {
                if (dbSet == null)
                {
                    dbSet = db.Set<T>();
                }
                return dbSet as DbSet<T>;
            }
        }
    }
}

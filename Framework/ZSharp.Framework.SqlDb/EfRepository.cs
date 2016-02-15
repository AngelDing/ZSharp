using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Data.Entity;
using ZSharp.Framework.Entities;
using ZSharp.Framework.Specifications;
using ZSharp.Framework.Repositories;

namespace ZSharp.Framework.SqlDb
{
    public class EfRepository<T, Tkey> : Repository<T, Tkey>
        where T : Entity<Tkey>, IAggregateRoot<Tkey>
    {
        private readonly IEfRepositoryContext efContext;
        private readonly DbContext db;
        private IDbSet<T> entities;

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
            var deleteEntity = this.GetByKey(key);
            this.Delete(deleteEntity);
        }

        public override void Delete(T entity)
        {
            if (entity is ISoftDeletable)
            {
                //TODO:可優化，不要HardCode。
                entity.NeedUpdateList.Add("IsDeleted", true);
                Update(entity);
            }
            else
            {
                efContext.RegisterDeleted(entity);
            }            
        }

        public override void Delete(Expression<Func<T, bool>> predicate)
        {
            var deleteList = this.GetBy(predicate);
            foreach (var e in deleteList)
            {
                this.Delete(e);
            }
        }

        public override bool Exists(Expression<Func<T, bool>> predicate)
        {
            var count = this.Entities.Count(predicate);
            return count != 0;
        }

        #endregion

        #region IReadOnlyRepository

        public override T GetByKey(Tkey key)
        {
            return this.Entities.Find(key);//.FirstOrDefault(p => (object)p.Id == (object)key);
        }

        public override IEnumerable<T> GetAll()
        {
            return this.Entities.AsNoTracking().ToList();
        }

        public override IEnumerable<T> GetBy(Expression<Func<T, bool>> predicate)
        {
            return this.Entities.Where(predicate).AsNoTracking().ToList();
        }

        public override IEnumerable<T> GetBy(ISpecification<T> spec)
        {
            return this.Entities.Where(spec.SatisfiedBy()).AsNoTracking().ToList();
        }

        public override T Single(Expression<Func<T, bool>> predicate)
        {
            return this.Entities.Single(predicate);
        }

        public override T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return this.Entities.FirstOrDefault(predicate);
        }

        public override int Count(Expression<Func<T, bool>> predicate)
        {
            return this.Entities.Count(predicate);
        }

        public override long LongCount(Expression<Func<T, bool>> predicate)
        {
            return this.Entities.LongCount(predicate);
        }

        #endregion     

        private DbSet<T> Entities
        {
            get
            {
                if (entities == null)
                {
                    entities = db.Set<T>();
                }
                return entities as DbSet<T>;
            }
        }
    }
}

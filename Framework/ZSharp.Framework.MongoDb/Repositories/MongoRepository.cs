using MongoDB.Driver;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using ZSharp.Framework.Specifications;
using ZSharp.Framework.Repositories;
using ZSharp.Framework.Entities;
using ZSharp.Framework.Validator;
using System.Threading.Tasks;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.MongoDb
{
    public class MongoRepository<T, TKey> : BaseMongoDB<T>, IMongoRepository<T, TKey>
      where T : BaseMongoEntity, IAggregateRoot<TKey>
    {
        public MongoRepository(string connectionStringOrName)
            : base(connectionStringOrName)
        {
        }

        #region IRepositoryContext

        /// <summary>
        /// 由于MongoDB暂不支持事务，且大部分操作是原子性的，故暂不实现UnitOfWork模式
        /// </summary>
        public IRepositoryContext RepoContext
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IReadOnlyRepository

        public T GetByKey(TKey id)
        {            
            return this.Collection.Find<T>(p => (object)p.Id == (object)id).FirstOrDefaultAsync().Result;
        }

        public IEnumerable<T> GetAll()
        {
            return this.Collection.Find<T>(p => true).ToListAsync().Result;
        }

        public IEnumerable<T> GetBy(Expression<Func<T, bool>> predicate)
        {
            return this.Collection.Find<T>(predicate).ToListAsync().Result;
        }

        public IEnumerable<T> GetBy(ISpecification<T> specification)
        {
            return this.GetBy(specification.SatisfiedBy());
        }

        #endregion

        #region IRepository

        public virtual void Insert(T entity)
        {
            AsyncHelper.RunSync(() => this.InsertAsync(entity));
        }

        public async Task InsertAsync(T entity)
        {
            Validate(entity);
            await this.Collection.InsertOneAsync(entity);
        }

        public void Insert(IEnumerable<T> entities)
        {
            AsyncHelper.RunSync(() => this.InsertAsync(entities));
        }

        public async Task InsertAsync(IEnumerable<T> entities)
        {
            Validate(entities);
            await this.Collection.InsertManyAsync(entities);
        }

        public void Update(T entity)
        {
            Update(entity, true);
        }
        private void Update(T entity, bool isNeedValidate)
        {
            AsyncHelper.RunSync(() => this.UpdateAsync(entity, isNeedValidate));
        }

        public async Task UpdateAsync(T entity, bool isNeedValidate)
        {
            if (isNeedValidate)
            {
                Validate(entity);
            }
            await this.Collection.ReplaceOneAsync<T>(p => (object)p.Id == (object)entity.Id, entity);
        }

        public void Update(IEnumerable<T> entities)
        {
            Validate(entities);
            foreach (T entity in entities)
            {
                this.Update(entity, false);
            }
        }

        public void Delete(TKey id)
        {
            AsyncHelper.RunSync(() => this.DeleteAsync(id));
        }

        public async Task DeleteAsync(TKey id)
        {
            await this.Collection.DeleteOneAsync<T>(p => (object)p.Id == (object)id);
        }

        public void Delete(T entity)
        {
            this.Delete(entity.Id);
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            var dList = this.GetBy(predicate).ToList();
            foreach (T entity in dList)
            {
                this.Delete(entity.Id);
            }
        }

        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            var obj = this.Collection.Find<T>(predicate).SingleOrDefaultAsync().Result;
            return obj != null;
        }

        #endregion

        #region IMongoRepository

        public void Update(Expression<Func<T, bool>> query, Dictionary<string, object> columnValues)
        {
            if (columnValues == null || columnValues.Count == 0)
            {
                throw new ArgumentException("Update Columns is Null!", "columnValues");
            }

            var updates = new List<UpdateDefinition<T>>();
            var builder = Builders<T>.Update;
            foreach (var key in columnValues.Keys)
            {
                if (string.IsNullOrEmpty(key))
                {
                    continue;
                }

                dynamic value = columnValues[key];
                var exp = GetExpression(key, value);
                var definition = builder.Set(exp, value);
                updates.Add(definition);
            }
            var update = builder.Combine(updates);
            AsyncHelper.RunSync(() => this.UpdateAsync(query, update));      
        }

        private Expression<Func<T, TField>> GetExpression<TField>(string key, TField value)
        {
            var keyArray = key.Split('.');
            var param = Expression.Parameter(typeof(T), "i");

            MemberExpression member = Expression.Property(param, keyArray[0]);
            for (var i = 1; i < keyArray.Length; i++)
            {
                member = Expression.Property(member, keyArray[i]);
            }

            var exp = Expression.Lambda<Func<T, TField>>(member, param);

            return exp;
        }

        public async Task UpdateAsync(Expression<Func<T, bool>> query, UpdateDefinition<T> update)
        {
            await this.Collection.UpdateOneAsync<T>(query, update);      
        }

        public void Update(Expression<Func<T, bool>> query, T entity)
        {
            Update(query, entity.NeedUpdateList);
        }

        public void RemoveAll()
        {
            this.Collection.DeleteManyAsync(p => true);
        }

        #endregion             

        private void Validate(T entity)
        {
            var validator = new EntityValidator();
            if (validator.IsValid(entity) == false)
            {
                throw new ValidationException(validator.GetInvalidMessages());
            }
        }

        private void Validate(IEnumerable<T> entities)
        {
            var validator = new EntityValidator();
            var errorList = new List<string>();
            foreach (var e in entities)
            {
                if (validator.IsValid(e) == false)
                {
                    errorList.AddRange(validator.GetInvalidMessages());
                }
            }
            if (errorList.Any())
            {
                throw new ValidationException(errorList);
            }
        }
    }
}

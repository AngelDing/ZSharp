using ZSharp.Framework.Specifications;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ZSharp.Framework.Entities;

namespace ZSharp.Framework.Repositories
{
    public interface IReadOnlyRepository<T, TKey> where T : IAggregateRoot<TKey>
    {
        T GetByKey(TKey key);

        IEnumerable<T> GetAll();

        IEnumerable<T> GetBy(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 規約模式：规约的主要作用是沟通，在.net中，规约的实现可以直接引用Lambda Expression，
        /// 具体见上面的GetBy方法，不仅实现简单，而且还能直接使用到ORM上以减小数据库查询开销。
        /// 对于其它的面向对象解决方案而言，规约的实现就不一定那么直观了。所以在实践中引入规约的概念还是很有必要的。
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        IEnumerable<T> GetBy(ISpecification<T> specification);

        T Single(Expression<Func<T, bool>> predicate);

        T FirstOrDefault(Expression<Func<T, bool>> predicate);

        int Count(Expression<Func<T, bool>> predicate);

        long LongCount(Expression<Func<T, bool>> predicate);
    }
}

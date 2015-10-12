using System;
using System.Linq.Expressions;

namespace ZSharp.Framework.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> SatisfiedBy();
    }
}

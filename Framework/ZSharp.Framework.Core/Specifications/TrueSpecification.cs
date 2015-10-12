using System;
using System.Linq.Expressions;

namespace ZSharp.Framework.Specifications
{
    public sealed class TrueSpecification<T> : Specification<T>
    {
        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            bool result = true;
            Expression<Func<T, bool>> trueExpression = t => result;
            return trueExpression;
        }
    }
}

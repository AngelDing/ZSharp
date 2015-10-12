using System;
using System.Linq;
using System.Linq.Expressions;

namespace ZSharp.Framework.Specifications
{
    public sealed class NotSpecification<T> : Specification<T>
    {
        private Expression<Func<T, bool>> originalCriteria;

        public NotSpecification(ISpecification<T> originalSpec)
        {
            if (originalSpec == null)
            {
                throw new ArgumentNullException("originalSpec");
            }
            originalCriteria = originalSpec.SatisfiedBy();
        }

        public NotSpecification(Expression<Func<T, bool>> originalSpec)
        {
            if (originalSpec == null)
            {
                throw new ArgumentNullException("originalSpec");
            }
            originalCriteria = originalSpec;
        }

        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            var body = Expression.Not(originalCriteria.Body);
            var parameters  = originalCriteria.Parameters.Single();
            return Expression.Lambda<Func<T, bool>>(body, parameters);
        }
    }
}

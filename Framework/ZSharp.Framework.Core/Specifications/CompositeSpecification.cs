
namespace ZSharp.Framework.Specifications
{
    public abstract class CompositeSpecification<T> : Specification<T>
    {
        public abstract ISpecification<T> LeftSpec { get; }

        public abstract ISpecification<T> RightSpec { get; }
    }
}

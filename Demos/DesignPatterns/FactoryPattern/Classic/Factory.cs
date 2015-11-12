using System;
using Common;

namespace FactoryPattern.Classic
{
    /// <summary>
    /// 实体工厂类型
    /// </summary>
    public class FactoryA : IFactory<IProductTest>
    {
        public IProductTest CreateProduct()
        {
            return new ProductA();
        }
    }

    /// <summary>
    /// 实体工厂类型
    /// </summary>
    public class FactoryB : IFactory<IProductTest>
    {
        public IProductTest CreateProduct()
        {
            return new ProductB();
        }
    }

    public class FactoryC : BaseFactory<ProductA>
    {
    }

    public class FactoryD : BaseFactory<IProductTest, CategoryType>
    {
        public FactoryD(CategoryType category)
            : base(category)
        {
        }

        public override IProductTest CreateProduct()
        {
            switch (Category)
            {
                case CategoryType.Good:
                    return new ProductA();
                case CategoryType.Bad:
                    return new ProductB();
                default:
                    return null;
            }
        }
    }

    public enum CategoryType
    {
        Good = 1,
        Bad = 2
    }
}

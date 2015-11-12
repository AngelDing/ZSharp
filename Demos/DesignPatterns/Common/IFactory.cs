using System;

namespace Common
{
    /// <summary>
    /// 工厂方法模式通用模板
    /// </summary>
    /// <typeparam name="T">要生产的产品</typeparam>
    /// <typeparam name="TCategory">根据不同类型，创建不同产品，此限定为枚举类型</typeparam>
    public interface IFactory<T, TCategory> : IFactory<T> where T : IProduct
    {
    }

    /// <summary>
    /// 工厂方法模式通用模板
    /// </summary>
    /// <typeparam name="T">要生产的产品</typeparam>
    public interface IFactory<T> where T : IProduct
    {
        T CreateProduct();
    }

    public abstract class BaseFactory<T> : IFactory<T> where T : IProduct, new()
    {
        public virtual T CreateProduct()
        {
            return new T();
        }
    }

    public abstract class BaseFactory<T, TCategory> : IFactory<T, TCategory> where T : IProduct
    {
        public TCategory Category { get; private set; }

        public BaseFactory(TCategory category)
        {
            var categoryType = category.GetType();
            if (!categoryType.IsEnum)
            {
                throw new ArgumentException("TCategory類型必須為枚舉！");
            }

            this.Category = category;
        }

        public abstract T CreateProduct();

    }
}

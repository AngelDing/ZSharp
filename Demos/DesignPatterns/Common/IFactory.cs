namespace Common
{
    /// <summary>
    /// 工厂方法模式通用模板
    /// </summary>
    /// <typeparam name="T">要生产的产品</typeparam>
    /// <typeparam name="TType">根据不同类型，创建不同产品，此限定为枚举类型</typeparam>
    public interface IFactory<T, TType> where T : IProduct
    {
        T CreateProduct(TType category);
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
}

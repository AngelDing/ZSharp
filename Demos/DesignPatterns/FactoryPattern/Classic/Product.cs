
using System;
using Common;

namespace FactoryPattern.Classic
{
    public interface IProductTest : IProduct
    {
        string Name { get; }

        void ShowName();
    }

    /// <summary>
    /// 具体产品类型
    /// </summary>
    public class ProductA : IProductTest
    {
        public string Name { get { return "A"; } }

        public void ShowName()
        {
            Console.WriteLine(Name);
        }
    }

    /// <summary>
    /// 具体产品类型
    /// </summary>
    public class ProductB : IProductTest
    {
        public string Name { get { return "B"; } }

        public void ShowName()
        {
            Console.WriteLine(Name);
        }
    }
}

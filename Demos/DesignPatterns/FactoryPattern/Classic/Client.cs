using Common;
using System;


namespace FactoryPattern.Classic
{
    class Client
    {
        private IFactory<IProductTest> factory;

        public Client(IFactory<IProductTest> factory)     // 将IFactory通过IOC方式注入
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            this.factory = factory;
        }

        public void SomeMethod()
        {
            var product = factory.CreateProduct();
            product.ShowName();
        }

        public static void Test()
        {
            var factory = new FactoryA();

            new Client(factory).SomeMethod();
        }
    }
}

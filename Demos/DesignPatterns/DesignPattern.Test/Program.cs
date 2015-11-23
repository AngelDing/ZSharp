using DesignPattern.Test.Visitor;
using System;

namespace DesignPattern.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //var observer = new ObserverTest();
            //observer.TestMulticst();
            //observer.TestMultiSubject();

            //new MementoClient().Test();

            //Console.WriteLine("-----Begin:状态模式测试-----");
            //new StateTest().Test();
            //Console.WriteLine("-----End:状态模式测试-----");

            //Console.WriteLine("-----Begin:策略模式测试-----");
            //new StrategyClient().Test();
            //Console.WriteLine("-----End:策略模式测试-----");

            Console.WriteLine("-----Begin:访问者模式测试-----");
            new VisitorTest().Test();
            Console.WriteLine("-----End:访问者模式测试-----");
            


            Console.ReadLine();
        }
    }
}

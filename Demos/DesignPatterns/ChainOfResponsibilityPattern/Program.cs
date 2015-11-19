using Common.BehavioralPatterns;
using System;
using System.Collections.Generic;

namespace ChainOfResponsibilityPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            new MediatorClient().Test();
            new MerdiatorDelegatingClient().Test();
            //ClassicTest();
            //BreakPointTest();
            Console.ReadLine();
        }

        public static void ClassicTest()
        {
            // 生成操作对象实例
            var handler1 = new InternalHandler();
            var handler2 = new DiscountHandler();
            var handler3 = new MailHandler();
            var handler4 = new RegularHandler();

            // 组装链式的结构  internal-> mail-> discount-> regular-> null
            handler1.Successors = new List<IHandler> { handler3 };
            handler3.Successors = new List<IHandler> { handler2 }; 
            handler2.Successors = new List<IHandler> { handler4 }; 
            var head = handler1;

            Request request = new Request(20, PurchaseType.Mail);
            head.Handle(request);
            Console.WriteLine(request.Price);
        }

        public static void BreakPointTest()
        {
            new BreakPointTest().Test();
        }
    }
}

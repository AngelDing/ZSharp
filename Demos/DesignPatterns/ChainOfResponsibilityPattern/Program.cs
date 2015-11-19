using Common.BehavioralPatterns;
using System;

namespace ChainOfResponsibilityPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            ClassicTest();
            BreakPointTest();
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
            handler1.Successor = handler3;
            handler3.Successor = handler2;
            handler2.Successor = handler4;
            var head = handler1;

            Request request = new Request(20, PurchaseType.Mail);
            head.Handle(request);
            Console.WriteLine(request.Price);

            // 重新组织链表结构
            handler1.Successor = handler1.Successor.Successors;  // 短路掉Discount
            request = new Request(20, PurchaseType.Discount);
            head.Handle(new Request(20, PurchaseType.Discount));
            Console.WriteLine(request.Price);    // 确认被短路的部分无法生效
        }

        public static void BreakPointTest()
        {
            new BreakPointTest().Test();
        }
    }
}

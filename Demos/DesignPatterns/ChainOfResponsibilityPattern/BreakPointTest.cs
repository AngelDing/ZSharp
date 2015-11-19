using Common.BehavioralPatterns;
using System;
using System.Collections.Generic;

namespace ChainOfResponsibilityPattern
{
    public class BreakPointTest
    {
        private PurchaseType currentType;

        public void Test()
        {
            // 生成操作对象实例、组装链式结构
            IHandler handler1 = new InternalHandler();
            IHandler handler2 = new DiscountHandler();
            IHandler handler3 = new MailHandler();
            IHandler handler4 = new RegularHandler();
            handler1.Successors = new List<IHandler> { handler3 };
            handler3.Successors = new List<IHandler> { handler2 };
            handler2.Successors = new List<IHandler> { handler4 };
            IHandler head = handler1;

            handler1.HasBreakPoint = true;
            handler1.Break += Break;
            handler3.HasBreakPoint = true;
            handler3.Break += Break;

            Request request = new Request(20, PurchaseType.Regular);
            head.Handle(request);
            Console.WriteLine(currentType.ToString());
            currentType = PurchaseType.Internal;    // 为第一个断点做的准备

            Console.WriteLine(request.Price);
            Console.WriteLine(currentType.ToString());
        }

        void Break(object sender, CallHandlerEventArgs args)
        {
            IHandler handler = args.Handler;

            currentType = PurchaseType.Mail;    // 为第二调用做的修改
            args.Handler.HasBreakPoint = false;
            args.Handler.Handle(args.Request);
        }
    }
}

using Common.BehavioralPatterns;
using System;

namespace DesignPattern.Test
{
    public class ObserverTest
    {
        /// <summary>
        /// 验证目标类型对观察者类型的1:N通知
        /// </summary>
        public void TestMulticst()
        {
            var subject = new SubjectBase<int>();
            Observer<int> observer1 = new Observer<int>();
            observer1.State = 10;
            Observer<int> observer2 = new Observer<int>();
            observer2.State = 20;

            // Attach Observer
            //subject += observer1;
            //subject += observer2;
            subject.AttachObserver(observer1);
            subject.AttachObserver(observer2);

            // 确认通知的有效性
            subject.Update(1);
            Console.WriteLine(observer1.State);  //1
            Console.WriteLine(observer2.State);  //1

            // 确认变更通知列表后的有效性
            //subject -= observer1;
            subject.DetachObserver(observer1);
            subject.Update(5);
            Console.WriteLine(observer1.State);  //1
            Console.WriteLine(observer2.State);  //5
        }

        /// <summary>
        /// 验证同一个观察者对象可以同时“观察”多个目标对象
        /// </summary>
        public void TestMultiSubject()
        {
            var subjectA = new SubjectBase<int>();
            var subjectB = new SubjectBase<int>();
            Observer<int> observer = new Observer<int>();
            observer.State = 20;
            subjectA += observer;
            subjectB += observer;

            subjectA.Update(10);
            Console.WriteLine(observer.State);  //10

            subjectB.Update(5);
            Console.WriteLine(observer.State);  //5
        }
    }
}

using System;

namespace DesignPattern.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var observer = new ObserverTest();
            observer.TestMulticst();
            observer.TestMultiSubject();


            new MementoClient().Test();

            Console.ReadLine();
        }
    }
}

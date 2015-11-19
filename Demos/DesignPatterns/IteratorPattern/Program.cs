using System;
using System.Collections.Generic;

namespace IteratorPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            new IteratorClient().TestCase1();

            RoundRobinTest();

            Console.ReadLine();
        }

        private static void RoundRobinTest()
        {
            JosephusRingInt32 aRing = new JosephusRingInt32(17, 0);
            aRing.RunToEnd();
        }
    }
}

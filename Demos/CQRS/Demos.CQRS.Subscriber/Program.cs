using System;

namespace Demos.CQRS.Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var processor = new CqrsDemoProcessor())
            {
                processor.Start();

                Console.WriteLine("Host started");
                Console.WriteLine("Press enter to finish");
                Console.ReadLine();

                processor.Stop();
            }
        }
    }
}

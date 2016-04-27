
using ZSharp.Framework;
using System.Collections.Concurrent;
using System;

namespace Framework.RabbitMq.Test
{
    public class BaseRabbitMqTest : DisposableObject
    {
        protected const string QueueName = "TesqQueue";
        protected ConcurrentBag<IDisposable> DisposableObjs;

        public BaseRabbitMqTest()
        {
            DisposableObjs = new ConcurrentBag<IDisposable>();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var d in DisposableObjs)
                {
                    d.Dispose();
                }
            }
        }

        protected MyTestMessage GetMyTestMessage()
        {
            return new MyTestMessage
            {
                Name = "Test Name",
                Price = 1000,
                Qty = 2
            };
        }
    }
}

using ZSharp.Framework;
using System.Collections.Concurrent;
using System;
using System.Threading.Tasks;
using System.Diagnostics;

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

        protected MyTestMessage GetMyTestMessage(string name = null)
        {
            if (name == null)
            {
                name = "Test";
            }
            return new MyTestMessage
            {
                Name = name,
                Price = 1000,
                Qty = 2
            };
        }

        private void MyTestMessageHandler(MyTestMessage message)
        {
            Debug.WriteLine(message.ToString());
        }

        protected Task MyTestMessageHandlerAsync(MyTestMessage message)
        {
            Action aa = () => MyTestMessageHandler(message);
            var task = new Task(aa);
            task.Start();
            return task;
        }

    }
}

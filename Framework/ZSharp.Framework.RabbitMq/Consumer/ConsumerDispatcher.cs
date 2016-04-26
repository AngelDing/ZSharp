using System;
using System.Collections.Concurrent;
using System.Threading;

namespace ZSharp.Framework.RabbitMq
{
    public interface IConsumerDispatcher : IDisposable
    {
        void QueueAction(Action action);
        void OnDisconnected();
    }

    public class ConsumerDispatcher : BaseRabbitMq, IConsumerDispatcher
    {
        private readonly BlockingCollection<Action> queue;
        private bool disposed;

        public ConsumerDispatcher()
        {
            queue = new BlockingCollection<Action>();

            var thread = new Thread(_ =>
            {
                Action action;
                while (!disposed && queue.TryTake(out action, -1))
                {
                    try
                    {
                        action();
                    }
                    catch (Exception exception)
                    {
                        Logger.Error("", exception);
                    }
                }
            }) {Name = "ZRabbitMq consumer dispatch thread", IsBackground = RabbitMqConfiguration.UseBackgroundThreads};
            thread.Start();
        }

        public void QueueAction(Action action)
        {
            queue.Add(action);
        }

        public void OnDisconnected()
        {
            // throw away any queued actions. RabbitMQ will redeliver any in-flight
            // messages that have not been acked when the connection is lost.
            Action result;
            while (queue.TryTake(out result))
            {
            }
        }

        public void Dispose()
        {
            queue.CompleteAdding();
            disposed = true;
        }

        public bool IsDisposed
        {
            get { return disposed; }
        }
    }
}
using ZSharp.Framework.Domain;
using System.Linq;
using System.Collections.Generic;

namespace ZSharp.Domain.Demo
{
    public class MemoryMessageBroker
    {
        private static Dictionary<string, Queue<IMessage>> msgQueueDic = new Dictionary<string, Queue<IMessage>>();

        public static void Publish(IMessage msg, string topic)
        {
            if (!msgQueueDic.Keys.Contains(topic))
            {
                msgQueueDic.Add(topic, new Queue<IMessage>());
            }
             msgQueueDic[topic].Enqueue(msg);
        }

        public static IMessage Subscribe(string topic)
        {
            IMessage msg = null;
            if (msgQueueDic.Keys.Contains(topic))
            {
                var msgQueue = msgQueueDic[topic];
                if (msgQueue.Count() > 0)
                {
                    msg = msgQueue.Dequeue();
                }
            }
            return msg;
        }
    }
}

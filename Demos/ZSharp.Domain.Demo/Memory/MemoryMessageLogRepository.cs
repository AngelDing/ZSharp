using ZSharp.Framework.Domain;
using System.Collections.Generic;

namespace ZSharp.Domain.Demo
{
    public class MemoryMessageLogRepository : IMessageLogRepository
    {
        private static IList<IMessage> msgLogs = new List<IMessage>();

        public void Save(IMessage msg)
        {
            msgLogs.Add(msg);
        }
    }
}

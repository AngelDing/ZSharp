using System;

namespace ZSharp.Framework.Domain
{
    public class SqlMessageLogRepository : IMessageLogRepository
    {
        public void Save(ICommand command)
        {
            throw new NotImplementedException();
        }

        public void Save(IEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}

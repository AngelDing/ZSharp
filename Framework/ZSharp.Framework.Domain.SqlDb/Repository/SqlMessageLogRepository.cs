using System;

namespace ZSharp.Framework.Domain
{
    public class SqlMessageLogRepository : BaseSqlDomainRepositroy, IMessageLogRepository
    {
        public void Save(IMessage msg)
        {
            throw new NotImplementedException();
        }
    }
}

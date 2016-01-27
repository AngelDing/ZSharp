using System;
using System.Linq;
using System.Collections.Generic;
using ZSharp.Framework.EfExtensions;

namespace ZSharp.Framework.Domain
{
    public class SqlMessageRepository<T> : BaseSqlDomainRepositroy, IMessageRepository<T> where T : MessageEntity
    {
        public void Delete(Guid id)
        {
            var msg = DB.Set<T>().FirstOrDefault(p => p.Id == id);
            if (msg != null)
            {
                DB.Delete(msg);
            }
        }

        public T GetFirstMessage(string sysCode, string topic)
        {
            var currentDate = DateTimeOffset.Now;
            var msg = DB.Set<T>()
                .Where(p => p.DeliveryDate <= currentDate && p.SysCode == sysCode && p.Topic == topic)
                .OrderBy(p => p.Id)
                .FirstOrDefault();
            return msg;
        }

        public void Insert(IEnumerable<T> messages)
        {
            DB.BulkInsert(messages);
        }

        public void Insert(T message)
        {
            DB.Insert(message);
        }
    }
}

using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using ZSharp.Framework.EfExtensions;

namespace ZSharp.Framework.Domain
{
    public class SqlMessageRepository<T> : IMessageRepository<T> where T : MessageEntity
    {
        private readonly DbContext db;

        public SqlMessageRepository()
        {
            this.db = new DomainDbContext();
        }

        public void Delete(long id)
        {
            var msg = db.Set<T>().FirstOrDefault(p => p.Id == id);
            if (msg != null)
            {
                db.Delete(msg);
            }
        }

        public T GetFirstMessage(string sysCode, string topic)
        {
            var currentDate = DateTimeOffset.UtcNow;
            var msg = db.Set<T>().Where(p => p.DeliveryDate <= currentDate && p.SysCode == sysCode && p.Topic == topic)
                .OrderBy(p => p.Id)
                .FirstOrDefault();
            return msg;
        }

        public void Insert(IEnumerable<T> messages)
        {
            db.BulkInsert(messages);
        }

        public void Insert(T message)
        {
            db.Insert(message);
        }
    }
}

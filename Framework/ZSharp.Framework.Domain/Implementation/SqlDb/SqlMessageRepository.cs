using System;
using System.Collections.Generic;

namespace ZSharp.Framework.Domain
{
    public class SqlMessageRepository<T> : IMessageRepository<T> where T : MessageEntity
    {
        public void Delete(long id)
        {
            throw new NotImplementedException();
        }

        public T GetFirstMessage()
        {
            var currentDate = GetCurrentDate();
            throw new NotImplementedException();
        }

        public void Insert(IEnumerable<T> messages)
        {
            throw new NotImplementedException();
        }

        public void Insert(T message)
        {
            throw new NotImplementedException();
        }

        protected virtual DateTimeOffset GetCurrentDate()
        {
            return DateTimeOffset.UtcNow;
        }
    }
}

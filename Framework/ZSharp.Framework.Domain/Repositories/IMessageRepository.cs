using System;
using System.Collections.Generic;

namespace ZSharp.Framework.Domain
{
    public interface IMessageRepository<T> where T : MessageEntity
    {
        void Insert(T message);

        void Insert(IEnumerable<T> messages);

        void Delete(long id);

        T GetFirstMessage(string sysCode, string topic);
    }
}

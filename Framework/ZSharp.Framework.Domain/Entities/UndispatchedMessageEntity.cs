using System;
using ZSharp.Framework.SqlDb;

namespace ZSharp.Framework.Domain
{
    public class UndispatchedMessageEntity : EfEntity<Guid>
    { 
        public string Commands { get; set; }
    }
}

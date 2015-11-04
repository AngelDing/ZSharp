using System;
using ZSharp.Framework.Entities;

namespace ZSharp.Framework.Domain
{
    public class UndispatchedMessageEntity : Entity<Guid>
    { 
        public string Commands { get; set; }
    }
}

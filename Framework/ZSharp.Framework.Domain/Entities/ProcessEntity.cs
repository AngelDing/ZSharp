using ZSharp.Framework.Entities;
using ZSharp.Framework.Utils;
using System;
using System.ComponentModel.DataAnnotations;

namespace ZSharp.Framework.Domain
{
    public class ProcessEntity : BaseDomainEntity, IAggregateRoot<Guid>
    {
        public ProcessEntity()
        {
            this.Id = GuidHelper.NewSequentialId();
        }

        public bool Completed { get; set; }

        public Guid ExpirationCommandId { get; set; }

        public int StateValue { get; set; }

        [ConcurrencyCheck]
        [Timestamp]
        public byte[] TimeStamp { get; private set; }
    }
}

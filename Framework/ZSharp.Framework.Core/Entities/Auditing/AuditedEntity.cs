using System;

namespace ZSharp.Framework.Entities
{
    [Serializable]
    public abstract class AuditedEntity<TKey, TUser> : CreationAuditedEntity<TKey, TUser>, IAudited<TUser>
    {
        public TUser ModifiedBy { get; set; }

        public DateTimeOffset? ModificationTime { get; set; }
    }
}
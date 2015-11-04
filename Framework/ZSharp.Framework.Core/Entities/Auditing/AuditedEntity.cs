using System;

namespace ZSharp.Framework.Entities
{
    [Serializable]
    public abstract class AuditedEntity<TKey, TUser> : CreationAuditedEntity<TKey, TUser>, IAudited<TUser>
    {
        public TUser LastModifiedBy { get; set; }

        public DateTimeOffset? LastModificationTime { get; set; }
    }
}
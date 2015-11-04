using System;

namespace ZSharp.Framework.Entities
{
    [Serializable]
    public abstract class FullAuditedEntity<TKey, TUser> : AuditedEntity<TKey, TUser>, IFullAudited<TUser>
    {
        public bool IsDeleted { get; set; }

        public TUser DeletedBy { get; set; }

        public DateTimeOffset? DeletionTime { get; set; }
    }
}
using System;

namespace ZSharp.Framework.Entities
{
    [Serializable]
    public abstract class CreationAuditedEntity<TKey, TUser> : Entity<TKey>, ICreationAudited<TUser>
    {
        protected CreationAuditedEntity()
        {
            CreationTime = DateTimeOffset.Now;
        }

        public TUser CreatedBy { get; set; }

        public DateTimeOffset CreationTime { get; set; }        
    }
}
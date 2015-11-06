using System;

namespace ZSharp.Framework.Entities
{
    public interface IDeletionAudited<TUser> : ISoftDeletable
    {
        TUser DeletedBy { get; set; }

        DateTimeOffset? DeletionTime { get; set; }
    }
}
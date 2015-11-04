using System;

namespace ZSharp.Framework.Entities
{
    public interface IDeletionAudited<TUser> : ISoftDelete
    {
        TUser DeletedBy { get; set; }

        DateTimeOffset? DeletionTime { get; set; }
    }
}
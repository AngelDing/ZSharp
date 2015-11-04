using System;

namespace ZSharp.Framework.Entities
{
    public interface IModificationAudited<TUser> 
    {
        TUser LastModifiedBy { get; set; }

        DateTimeOffset? LastModificationTime { get; set; }
    }
}
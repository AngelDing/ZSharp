using System;

namespace ZSharp.Framework.Entities
{
    public interface IModificationAudited<TUser> 
    {
        TUser ModifiedBy { get; set; }

        DateTimeOffset? ModificationTime { get; set; }
    }
}
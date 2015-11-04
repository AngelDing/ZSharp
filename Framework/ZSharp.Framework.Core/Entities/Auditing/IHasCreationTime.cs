using System;

namespace ZSharp.Framework.Entities
{
    public interface IHasCreationTime
    {
        DateTimeOffset CreationTime { get; set; }
    }
}
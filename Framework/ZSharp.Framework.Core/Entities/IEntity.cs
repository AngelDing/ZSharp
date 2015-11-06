using System;

namespace ZSharp.Framework.Entities
{
    public interface IEntity<TKey> : IEntity
    {
        TKey Id { get; set; }

        bool IsTransient();
    }

    public interface IEntity
    {
    }
}

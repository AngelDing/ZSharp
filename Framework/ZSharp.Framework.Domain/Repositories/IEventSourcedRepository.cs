using System;

namespace ZSharp.Framework.Domain
{
    public interface IEventSourcedRepository<T> where T : IEventSourced
    {
        T Get(Guid id);

        void Save(T eventSourced);
    }
}

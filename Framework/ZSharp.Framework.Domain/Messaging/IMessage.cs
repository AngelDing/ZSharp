
using System;

namespace ZSharp.Framework.Domain
{
    public interface IMessage
    {
        Guid Id { get; }
    }

    public interface ICommand : IMessage
    {
    }

    public interface IEvent : IMessage
    {
    }
}

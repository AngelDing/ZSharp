using System;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.Domain
{
    public class Message : IMessage
    {
        public Message()
        {
            this.Id = GuidHelper.NewSequentialId();
        }

        public Guid Id { get; private set; }
    }

    public class Command : Message, ICommand
    {
    }

    public class Event : Message, IEvent
    {
    }
}

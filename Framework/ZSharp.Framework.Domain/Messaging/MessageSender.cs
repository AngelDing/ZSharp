using System.Collections.Generic;

namespace ZSharp.Framework.Domain
{   
    public abstract class MessageSender : IMessageSender
    {
        public abstract void Send(Message message);

        public abstract void Send(IEnumerable<Message> messages);
    }
}

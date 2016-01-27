using System.Collections.Generic;

namespace ZSharp.Framework.Domain
{   
    public abstract class MessageSender : IMessageSender
    {
        public abstract void Send<T>(Envelope<T> message) where T : IMessage;

        public abstract void Send<T>(IEnumerable<Envelope<T>> messages) where T : IMessage;
    }
}

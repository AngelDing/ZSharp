using System;
using System.Collections.Generic;

namespace ZSharp.Framework.Domain
{   
    public abstract class MessageSender : IMessageSender
    {
        public void Send(Envelope<IMessage> message)
        {
            throw new NotImplementedException();
        }

        public void Send(IEnumerable<Envelope<IMessage>> message)
        {
            throw new NotImplementedException();
        }

        public abstract void Send(Message message);

        public abstract void Send(IEnumerable<Message> messages);
    }
}

using System;
using System.Collections.Generic;

namespace ZSharp.Framework.Domain
{
    public class RabbitMqMessageSender : MessageSender
    {
        public override void Send(IEnumerable<Message> messages)
        {
            throw new NotImplementedException();
        }

        public override void Send(Message message)
        {
            throw new NotImplementedException();
        }
    }
}

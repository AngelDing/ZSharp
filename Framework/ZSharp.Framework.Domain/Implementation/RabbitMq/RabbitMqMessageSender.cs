using System;
using System.Collections.Generic;

namespace ZSharp.Framework.Domain
{
    public class RabbitMqMessageSender : MessageSender
    {
        public override void Send<T>(Envelope<T> message)
        {
            throw new NotImplementedException();
        }

        public override void Send<T>(IEnumerable<Envelope<T>> messages)
        {
            throw new NotImplementedException();
        }
    }
}

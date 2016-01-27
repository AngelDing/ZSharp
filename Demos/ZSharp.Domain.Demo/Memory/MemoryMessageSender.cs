using ZSharp.Framework.Domain;
using System;
using System.Collections.Generic;

namespace ZSharp.Domain.Demo
{
    public class MemoryMessageSender : MessageSender
    {
        public override void Send<T>(IEnumerable<Envelope<T>> messages)
        {
            foreach (var msg in messages)
            {
                this.Send(msg);
            }
        }

        public override void Send<T>(Envelope<T> message)
        {
            MemoryMessageBroker.Publish(message.Body, message.Topic);
        }
    }
}

using System.Collections.Generic;

namespace ZSharp.Framework.Domain
{
    public class RabbitMqMessageSender : MessageSender
    {
        public override void Send<T>(Envelope<T> message)
        {
            //using (var bus = RabbitHutch.CreateBus("host=localhost"))
            //{
            //    bus.Publish(message);
            //}
        }

        public override void Send<T>(IEnumerable<Envelope<T>> messages)
        {
            //using (var bus = RabbitHutch.CreateBus("host=localhost"))
            //{
            //    bus.Publish(messages);
            //}
        }
    }
}

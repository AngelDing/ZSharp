using ZSharp.Framework.Domain;
using ZSharp.Framework.Infrastructure;
using System.Collections.Generic;

namespace ZSharp.Domain.Demo
{
    public class MemoryMessageReceiver : MessageReceiver
    {
        public MemoryMessageReceiver(IEnumerable<string> topics)
            : base(topics)
        {
        }

        protected override bool ReceiveMessage()
        {
            foreach (var topic in this.Topics)
            {
                var msgEntity = MemoryMessageBroker.Subscribe(topic); ;
                if (msgEntity != null)
                {
                    var message = msgEntity.Map<Envelope<IMessage>>();
                    this.MessageReceived(new MessageReceivedEventArgs(message));
                }
            }
            return true;
        }
    }
}

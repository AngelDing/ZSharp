using EasyNetQ;
using System;
using System.Collections.Generic;

namespace ZSharp.Framework.Domain
{
    public class RabbitMqMessageReceiver : MessageReceiver
    {
        public RabbitMqMessageReceiver()
            : this(null)
        {
        }

        public RabbitMqMessageReceiver(IEnumerable<string> topics)
            : base(topics)
        {
        }

        protected override bool ReceiveMessage()
        {
            var bus = RabbitHutch.CreateBus("host=localhost");
            foreach (var topic in this.Topics)
            {
                bus.Subscribe<Envelope<IMessage>>(SysCode, HandleWaitMessage, p => p.WithTopic(topic));
            }
            return true;
        }

        private void HandleWaitMessage(Envelope<IMessage> msg)
        {
            try
            {
                this.MessageReceived(new MessageReceivedEventArgs(msg));
                //this.msgRepo.Delete(messageId);
            }
            catch (Exception ex)
            {
                //TODO:記錄日誌
                throw ex;
            }
        }
    }
}

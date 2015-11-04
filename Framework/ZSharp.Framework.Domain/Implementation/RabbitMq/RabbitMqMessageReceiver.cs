using EasyNetQ;
using System;

namespace ZSharp.Framework.Domain
{
    public class RabbitMqMessageReceiver : MessageReceiver
    {
        public RabbitMqMessageReceiver(string sysCode, string topic)
            : base(sysCode, topic)
        {
        }

        protected override bool ReceiveMessage()
        {
            var bus = RabbitHutch.CreateBus("host=localhost");
            bus.Subscribe<Envelope<IMessage>>(SysCode, HandleWaitMessage, p => p.WithTopic(Topic));
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

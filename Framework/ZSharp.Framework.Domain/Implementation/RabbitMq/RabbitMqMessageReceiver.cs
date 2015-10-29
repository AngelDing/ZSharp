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
            throw new NotImplementedException();
        }
    }
}

using System;

namespace ZSharp.Framework.Domain
{
    public class RabbitMqMessageReceiver : MessageReceiver
    {
        public RabbitMqMessageReceiver()
            : base()
        {
        }

        protected override bool ReceiveMessage()
        {
            throw new NotImplementedException();
        }
    }
}

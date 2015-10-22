using System;

namespace ZSharp.Framework.Domain
{
    public class RabbitMqMessageReceiver : MessageReceiver
    {
        public RabbitMqMessageReceiver()
            : base(TimeSpan.FromMilliseconds(100))
        {
        }

        protected override bool ReceiveMessage()
        {
            throw new NotImplementedException();
        }
    }
}

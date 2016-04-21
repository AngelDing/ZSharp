using System;

namespace ZSharp.Framework.RabbitMq
{
    public class MessageReturnedEventArgs : EventArgs
    {
        public byte[] MessageBody { get; private set; }

        public MessageProperties MessageProperties { get; private set; }

        public MessageReturnedInfo MessageReturnedInfo { get; private set; }

        public MessageReturnedEventArgs(
            byte[] messageBody, 
            MessageProperties messageProperties,
            MessageReturnedInfo messageReturnedInfo)
        {
            MessageBody = messageBody;
            MessageProperties = messageProperties;
            MessageReturnedInfo = messageReturnedInfo;
        }
    }
}
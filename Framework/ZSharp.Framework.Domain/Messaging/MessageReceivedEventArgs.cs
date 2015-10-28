using System;

namespace ZSharp.Framework.Domain
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public MessageReceivedEventArgs(Envelope<IMessage> message)
        {
            this.Message = message;
        }

        public Envelope<IMessage> Message { get; private set; }
    }
}

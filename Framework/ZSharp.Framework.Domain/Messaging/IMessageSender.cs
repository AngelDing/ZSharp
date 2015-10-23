using System.Collections.Generic;

namespace ZSharp.Framework.Domain
{  
    /// <summary>
    /// Abstracts the behavior of sending a message.
    /// </summary>
    public interface IMessageSender
    {
        /// <summary>
        /// Sends the specified message.
        /// </summary>
        void Send(Message message);

        /// <summary>
        /// Sends a batch of messages.
        /// </summary>
        void Send(IEnumerable<Message> messages);

        void Send(Envelope<IMessage> message);

        void Send(IEnumerable<Envelope<IMessage>> message);
    }
}

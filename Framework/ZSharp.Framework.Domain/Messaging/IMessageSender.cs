using System.Collections.Generic;

namespace ZSharp.Framework.Domain
{  
    public interface IMessageSender
    {
        void Send<T>(Envelope<T> message) where T : IMessage;

        void Send<T>(IEnumerable<Envelope<T>> messages) where T : IMessage;
    }
}

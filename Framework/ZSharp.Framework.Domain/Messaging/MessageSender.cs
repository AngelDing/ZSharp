using System.Collections.Generic;
using ZSharp.Framework.Infrastructure;

namespace ZSharp.Framework.Domain
{   
    public class MessageSender : IMessageSender
    {
        private readonly IMessageRepository<MessageEntity> msgRepo;

        public MessageSender(IMessageRepository<MessageEntity> msgRepo)
        {
            this.msgRepo = msgRepo;
        }

        /// <summary>
        /// Sends the specified message.
        /// </summary>
        public void Send(Message message)
        {
            var msgEntity = message.Map<MessageEntity>();
            msgRepo.Insert(msgEntity);
        }

        /// <summary>
        /// Sends a batch of messages.
        /// </summary>
        public void Send(IEnumerable<Message> messages)
        {
            var msgEntities = messages.Map<IEnumerable<MessageEntity>>();
            msgRepo.Insert(msgEntities);
        }
    }
}

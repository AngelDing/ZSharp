using System.Collections.Generic;
using ZSharp.Framework.Infrastructure;

namespace ZSharp.Framework.Domain
{   
    public class SqlMessageSender : MessageSender
    {
        private readonly IMessageRepository<MessageEntity> msgRepo;

        public SqlMessageSender(IMessageRepository<MessageEntity> msgRepo)
        {
            this.msgRepo = msgRepo;
        }

        public override void Send(Message message)
        {
            var msgEntity = message.Map<MessageEntity>();
            msgRepo.Insert(msgEntity);
        }

        public override void Send(IEnumerable<Message> messages)
        {
            var msgEntities = messages.Map<IEnumerable<MessageEntity>>();
            msgRepo.Insert(msgEntities);
        }
    }
}

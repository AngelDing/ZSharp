using System.Collections.Generic;
using ZSharp.Framework.Infrastructure;
using ZSharp.Framework.Serializations;

namespace ZSharp.Framework.Domain
{
    public class SqlMessageSender : MessageSender
    {
        private readonly IMessageRepository<MessageEntity> msgRepo;
        private readonly ISerializer serializer;

        public SqlMessageSender(IMessageRepository<MessageEntity> msgRepo)
        {
            this.msgRepo = msgRepo;
            this.serializer = ServiceLocator.GetInstance<ISerializer>();
        }

        public override void Send<T>(Envelope<T> message)
        {
            var msgEntity = CreateMessageEntity(message);
            msgRepo.Insert(msgEntity);
        }

        public override void Send<T>(IEnumerable<Envelope<T>> messages)
        {
            var msgEntities = new List<MessageEntity>();
            foreach (var msg in messages)
            {
                msgEntities.Add(CreateMessageEntity(msg));
            }
            msgRepo.Insert(msgEntities);
        }

        private MessageEntity CreateMessageEntity<T>(Envelope<T> message)
        {
            var msgEntity = message.Map<MessageEntity>();
            msgEntity.BodyTypeName = typeof(T).AssemblyQualifiedName;
            msgEntity.Body = this.serializer.Serialize<string>(message.Body);
            return msgEntity;
        }
    }
}

using System.Collections.Generic;
using ZSharp.Framework.Infrastructure;
using ZSharp.Framework.Serializations;

namespace ZSharp.Framework.Domain
{
    public class SqlMessageSender<TEntity> : MessageSender where TEntity : MessageEntity
    {
        private readonly IMessageRepository<TEntity> msgRepo;
        private readonly ISerializer serializer;

        public SqlMessageSender(IMessageRepository<TEntity> msgRepo, ISerializer serializer)
        {
            this.msgRepo = msgRepo;
            this.serializer = serializer;
        }

        public override void Send<T>(Envelope<T> message)
        {
            var msgEntity = CreateMessageEntity(message);
            msgRepo.Insert(msgEntity);
        }

        public override void Send<T>(IEnumerable<Envelope<T>> messages)
        {
            var msgEntities = new List<TEntity>();
            foreach (var msg in messages)
            {
                msgEntities.Add(CreateMessageEntity(msg));
            }
            msgRepo.Insert(msgEntities);
        }

        private TEntity CreateMessageEntity<T>(Envelope<T> message)
        {
            var msgEntity = message.Map<TEntity>();
            msgEntity.BodyTypeName = typeof(T).AssemblyQualifiedName;
            msgEntity.Body = this.serializer.Serialize<string>(message.Body);
            return msgEntity;
        }
    }
}

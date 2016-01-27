using System;
using ZSharp.Framework.Infrastructure;
using System.Collections.Generic;

namespace ZSharp.Framework.Domain
{  
    public class SqlMessageReceiver<TEntity> : MessageReceiver where TEntity : MessageEntity
    {
        private readonly IMessageRepository<TEntity> msgRepo;
        public SqlMessageReceiver()
            : this(null)
        {
        }

        public SqlMessageReceiver(IEnumerable<string> topics)
            : base(topics)
        {
            this.msgRepo = ServiceLocator.GetInstance<IMessageRepository<TEntity>>();
        }      

        protected override bool ReceiveMessage()
        {
            foreach (var topic in this.Topics)
            {
                ReceiveMessage(topic);
            }
            return true;
        }

        private void ReceiveMessage(string topic)
        {
            using (var transaction = TransactionScopeFactory.CreateReadCommittedScope())
            {
                try
                {
                    var msgEntity = this.msgRepo.GetFirstMessage(this.SysCode, topic);
                    var message = msgEntity.Map<Envelope<IMessage>>();
                    var messageId = msgEntity.Id;
                    this.MessageReceived(new MessageReceivedEventArgs(message));
                    this.msgRepo.Delete(messageId);
                }
                catch (Exception ex)
                {
                    //TODO:記錄日誌
                    throw ex;
                }
                transaction.Complete();
            }
        }
    }
}

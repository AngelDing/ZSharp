using System;
using ZSharp.Framework.Infrastructure;

namespace ZSharp.Framework.Domain
{  
    public class SqlMessageReceiver<TEntity> : MessageReceiver where TEntity : MessageEntity
    {
        private readonly IMessageRepository<TEntity> msgRepo;

        public SqlMessageReceiver(IMessageRepository<TEntity> msgRepo, string sysCode, string topic)
            : base(sysCode, topic)
        {
            this.msgRepo = msgRepo;
        }      

        protected override bool ReceiveMessage()
        {
            using (var transaction = TransactionScopeFactory.CreateReadCommittedScope())
            {
                try
                {
                    var msgEntity = this.msgRepo.GetFirstMessage(this.SysCode, this.Topic);
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
            return true;
        }
    }
}

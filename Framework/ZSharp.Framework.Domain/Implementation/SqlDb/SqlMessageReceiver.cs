using System;
using ZSharp.Framework.Infrastructure;

namespace ZSharp.Framework.Domain
{  
    public class SqlMessageReceiver : MessageReceiver
    {
        private readonly IMessageRepository<MessageEntity> msgRepo;

        public SqlMessageReceiver(IMessageRepository<MessageEntity> msgRepo)
            : base()
        {
            this.msgRepo = msgRepo;
        }      

        protected override bool ReceiveMessage()
        {
            using (var transaction = TransactionScopeFactory.CreateReadCommittedScope())
            {
                try
                {
                    var msgEntity = this.msgRepo.GetFirstMessage();
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

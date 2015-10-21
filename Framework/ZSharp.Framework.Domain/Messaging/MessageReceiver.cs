using System;
using System.Threading;
using System.Threading.Tasks;
using ZSharp.Framework.Infrastructure;

namespace ZSharp.Framework.Domain
{  
    public class MessageReceiver : DisposableObject, IMessageReceiver, IDisposable
    {
        private readonly TimeSpan pollDelay;
        private readonly object lockObject = new object();
        private CancellationTokenSource cancellationSource;

        private readonly IMessageRepository<MessageEntity> msgRepo;

        public MessageReceiver(IMessageRepository<MessageEntity> msgRepo)
            : this(msgRepo, TimeSpan.FromMilliseconds(100))
        {
        }

        public MessageReceiver(IMessageRepository<MessageEntity> msgRepo, TimeSpan pollDelay)
        {
            this.pollDelay = pollDelay;
            this.msgRepo = msgRepo;            
        }

        public event EventHandler<MessageReceivedEventArgs> MessageReceived = (sender, args) => { };

        public void Start()
        {
            lock (this.lockObject)
            {
                if (this.cancellationSource == null)
                {
                    this.cancellationSource = new CancellationTokenSource();
                    Task.Factory.StartNew(
                        () => this.ReceiveMessages(this.cancellationSource.Token),
                        this.cancellationSource.Token,
                        TaskCreationOptions.LongRunning,
                        TaskScheduler.Current);
                }
            }
        }

        public void Stop()
        {
            lock (this.lockObject)
            {
                using (this.cancellationSource)
                {
                    if (this.cancellationSource != null)
                    {
                        this.cancellationSource.Cancel();
                        this.cancellationSource = null;
                    }
                }
            }
        }

        /// <summary>
        /// Receives the messages in an endless loop.
        /// </summary>
        private void ReceiveMessages(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (!this.ReceiveMessage())
                {
                    Thread.Sleep(this.pollDelay);
                }
            }
        }

        protected bool ReceiveMessage()
        {
            using (var transaction = TransactionScopeFactory.CreateReadCommittedScope())
            {
                try
                {
                    var msgEntity = this.msgRepo.GetFirstMessage();
                    var message = msgEntity.Map<Message>();
                    var messageId = msgEntity.Id;
                    this.MessageReceived(this, new MessageReceivedEventArgs(message));
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Stop();
            }
        }
    }
}

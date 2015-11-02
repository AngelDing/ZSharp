using System;
using ZSharp.Framework.Logging;

namespace ZSharp.Framework.Domain
{
    public class MessageProcessor : DisposableObject, IProcessor
    {
        private readonly MessageDispatcher msgDispatcher;
        private readonly IMessageReceiver receiver;
        private readonly object lockObject = new object();
        private bool disposed;
        private bool started = false;

        internal ILogger Logger { get; private set; }

        public MessageProcessor(IMessageReceiver receiver)
        {
            this.receiver = receiver;
            this.msgDispatcher = new MessageDispatcher();
            this.Logger = LogManager.GetLogger(this.GetType());
        }

        /// <summary>
        /// Starts the listener.
        /// </summary>
        public virtual void Start()
        {
            lock (this.lockObject)
            {
                if (!this.started)
                {
                    this.receiver.OnMessageReceived += OnMessageReceived;
                    this.receiver.Start();
                    this.started = true;
                }
            }
        }

        /// <summary>
        /// Stops the listener.
        /// </summary>
        public virtual void Stop()
        {
            lock (this.lockObject)
            {
                if (this.started)
                {
                    this.receiver.Stop();
                    this.receiver.OnMessageReceived -= OnMessageReceived;
                    this.started = false;
                }
            }
        }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs args)
        {
            Logger.Trace(new string('-', 100));

            try
            {
                //TODO：一條消息可能會引發多個Handler處理相關數據，如果其中有一個出現錯誤，則這條消息不會刪除，
                //下次會再次處理，此時的數據一致性如何解決？需要保證每個Handler都是冥等的？
                //Logger.Trace(this.Serialize(body));

                this.msgDispatcher.DispatchMessage(args.Message.Body, args.Message.CorrelationId);

                //Logger.Trace(new string('-', 100));
            }
            catch (Exception e)
            {
                // NOTE: we catch ANY exceptions as this is for local 
                // development/debugging. The Windows Azure implementation 
                // supports retries and dead-lettering, which would 
                // be totally overkill for this alternative debug-only implementation.
                Logger.Error("An exception happened while processing message through handler/s:\r\n{0}", e, this.GetType().FullName);
                Logger.Warn("Error will be ignored and message receiving will continue.");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.Stop();
                    this.disposed = true;

                    using (this.receiver as IDisposable)
                    {
                        // Dispose receiver if it's disposable.
                    }
                }
            }
        }

        public void Register(IHandler handler)
        {
            this.msgDispatcher.Register(handler);
        }
    }
}

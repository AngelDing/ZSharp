using System;
using System.Threading;
using System.Threading.Tasks;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.Domain
{  
    public abstract class MessageReceiver : DisposableObject, IMessageReceiver, IDisposable
    {
        private readonly TimeSpan pollDelay;
        private readonly object lockObject = new object();
        private CancellationTokenSource cancellationSource;
        internal string SysCode { get; private set; }
        internal string Topic { get; private set; }

        public MessageReceiver(string sysCode, string topic)
        {
            GuardHelper.ArgumentNotEmpty(() => sysCode);
            GuardHelper.ArgumentNotEmpty(() => topic);

            this.SysCode = sysCode;
            this.Topic = topic;
            this.pollDelay = TimeSpan.FromMilliseconds(100);
        }

        public event EventHandler<MessageReceivedEventArgs> OnMessageReceived = (sender, args) => { };

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

        protected void MessageReceived(MessageReceivedEventArgs args)
        {
            if (OnMessageReceived != null)
            {
                OnMessageReceived(this, args);
            }
        }

        protected abstract bool ReceiveMessage();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Stop();
            }
        }
    }
}

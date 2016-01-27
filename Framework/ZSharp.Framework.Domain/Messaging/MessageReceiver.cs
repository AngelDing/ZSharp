using System;
using System.Threading;
using System.Threading.Tasks;
using ZSharp.Framework.Extensions;
using System.Collections.Generic;
using ZSharp.Framework.Configurations;

namespace ZSharp.Framework.Domain
{  
    public abstract class MessageReceiver : DisposableObject, IMessageReceiver
    {
        private readonly TimeSpan pollDelay;
        private readonly object lockObject = new object();
        private CancellationTokenSource cancellationSource;
        public string SysCode { get; private set; }
        public IEnumerable<string> Topics { get; private set; }

        /// <summary>
        /// 消息接收器
        /// </summary>
        /// <param name="topics">关注的事件主题集合，虽然支持集合，但建议每个Processor仅处理一个主题</param>
        public MessageReceiver(IEnumerable<string> topics)
        {
            if (topics.IsNullOrEmpty())
            {
                topics = new List<string>
                {
                    Constants.ApplicationRuntime.DefaultCommandTopic,
                    Constants.ApplicationRuntime.DefaultEventTopic
                };
            }
            this.SysCode = CommonConfig.SystemCode;
            this.Topics = topics;
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

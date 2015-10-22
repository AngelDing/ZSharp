using System;
using ZSharp.Framework.Serializations;
using ZSharp.Framework.Logging;

namespace ZSharp.Framework.Domain
{
    /// <summary>
    /// Provides basic common processing code for components that handle 
    /// incoming messages from a receiver.
    /// </summary>
    public abstract class MessageProcessor : DisposableObject, IProcessor
    {
        private readonly IMessageReceiver receiver;
        private readonly ISerializer serializer;
        private readonly object lockObject = new object();
        private bool disposed;
        private bool started = false;

        internal ILogger Logger { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessor"/> class.
        /// </summary>
        protected MessageProcessor(IMessageReceiver receiver, ISerializer serializer)
        {
            this.receiver = receiver;
            this.serializer = serializer;
            this.Logger = LogManager.GetLogger(this.GetType());
        }

        /// <summary>
        /// Starts the listener.
        /// </summary>
        public virtual void Start()
        {
            ThrowIfDisposed();
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


        protected abstract void ProcessMessage(object payload, string correlationId);

        private void OnMessageReceived(object sender, MessageReceivedEventArgs args)
        {
            Logger.Trace(new string('-', 100));

            try
            {
                var body = Deserialize(args.Message);

                Logger.Trace(this.Serialize(body));
                Logger.Trace("");

                ProcessMessage(body, args.Message.CorrelationId);

                Logger.Trace(new string('-', 100));
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

        protected object Deserialize(Message msg)
        {
            var serializedPayload = msg.Body;
            var type = Type.GetType(msg.MessageType);
            return this.serializer.Deserialize(serializedPayload, type);
        }

        protected string Serialize(object payload)
        {
            return this.serializer.Serialize<string>(payload);
        }

        private void ThrowIfDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("MessageProcessor");
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
    }
}

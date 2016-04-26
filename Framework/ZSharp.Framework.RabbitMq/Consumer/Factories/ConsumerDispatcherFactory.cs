using System;

namespace ZSharp.Framework.RabbitMq
{
    public interface IConsumerDispatcherFactory : IDisposable
    {
        IConsumerDispatcher GetConsumerDispatcher();

        void OnDisconnected();
    }

    /// <summary>
    /// The default ConsumerDispatcherFactory. It creates a single dispatch
    /// queue which all consumers share.
    /// </summary>
    public class ConsumerDispatcherFactory : IConsumerDispatcherFactory
    {
        private readonly Lazy<IConsumerDispatcher> dispatcher;

        public ConsumerDispatcherFactory()
        {            
            dispatcher = new Lazy<IConsumerDispatcher>(() => new ConsumerDispatcher());
        }

        public IConsumerDispatcher GetConsumerDispatcher()
        {
            return dispatcher.Value;
        }

        public void OnDisconnected()
        {
            if (dispatcher.IsValueCreated)
            {
                dispatcher.Value.OnDisconnected();
            }
        }

        public void Dispose()
        {
            if (dispatcher.IsValueCreated)
            {
                dispatcher.Value.Dispose();
            }
        }
    }
}
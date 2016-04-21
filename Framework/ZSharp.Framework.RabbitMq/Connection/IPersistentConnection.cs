using RabbitMQ.Client;

namespace ZSharp.Framework.RabbitMq
{
    public interface IPersistentConnection
    {
        bool IsConnected { get; }

        /// <summary>
        /// Initialization method that should be called only once,
        /// usually right after the implementation constructor has run.
        /// </summary>
        void Initialize();

        IModel CreateModel();
    }
}
using System;
using System.Threading.Tasks;

namespace ZSharp.Framework.RabbitMq
{
    public interface IBus : IDisposable
    {
        void Publish<T>(T message) where T : class;

        void Publish<T>(T message, string topic) where T : class;

        Task PublishAsync<T>(T message) where T : class;

        Task PublishAsync<T>(T message, string topic) where T : class;
    }
}

using System;
using System.Threading.Tasks;

namespace ZSharp.Framework.RabbitMq
{
    public interface ISendReceive
    {
        void Send<T>(string queue, T message) where T : class;

        Task SendAsync<T>(string queue, T message) where T : class;

        IDisposable Receive<T>(string queue, Func<T, Task> onMessage, Action<IConsumerConfiguration> configureconfigure = null) 
            where T : class;

        IDisposable Receive(string queue, Action<IReceiveRegistration> addHandlers, Action<IConsumerConfiguration> configure = null);
    }
}
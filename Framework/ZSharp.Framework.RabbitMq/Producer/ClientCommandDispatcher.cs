using System;
using System.Threading.Tasks;
using RabbitMQ.Client;
using ZSharp.Framework.Configurations;

namespace ZSharp.Framework.RabbitMq
{
    /// <summary>
    /// Invokes client commands on a single channel. All commands are marshalled onto a single thread.
    /// </summary>
    public class ClientCommandDispatcher : IClientCommandDispatcher
    {
        private readonly Lazy<IClientCommandDispatcher> dispatcher;

        public ClientCommandDispatcher(IRabbitMqConfiguration configuration, IPersistentConnection connection, IPersistentChannelFactory persistentChannelFactory)
        {
            dispatcher = new Lazy<IClientCommandDispatcher>(
                () => new ClientCommandDispatcherSingleton(configuration, connection, persistentChannelFactory));
        }

        public T Invoke<T>(Func<IModel, T> channelAction)
        {
            return dispatcher.Value.Invoke(channelAction);
        }

        public void Invoke(Action<IModel> channelAction)
        {
            dispatcher.Value.Invoke(channelAction);
        }

        public Task<T> InvokeAsync<T>(Func<IModel, T> channelAction)
        {
            return dispatcher.Value.InvokeAsync(channelAction);
        }

        public Task InvokeAsync(Action<IModel> channelAction)
        {
            return dispatcher.Value.InvokeAsync(channelAction);
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
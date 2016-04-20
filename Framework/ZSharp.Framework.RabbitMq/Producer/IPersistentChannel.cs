using System;
using RabbitMQ.Client;

namespace ZSharp.Framework.RabbitMq
{
    public interface IPersistentChannel : IDisposable
    {
        void InvokeChannelAction(Action<IModel> channelAction);
    }
}
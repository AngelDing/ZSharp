using System;

namespace ZSharp.Framework.RabbitMq
{
    public interface IConsumer : IDisposable
    {
        IDisposable StartConsuming();
    }
}
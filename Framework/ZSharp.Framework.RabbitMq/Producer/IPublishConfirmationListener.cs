using System;
using RabbitMQ.Client;

namespace ZSharp.Framework.RabbitMq
{
    public interface IPublishConfirmationListener : IDisposable
    {
        IPublishConfirmationWaiter GetWaiter(IModel model);
    }
}
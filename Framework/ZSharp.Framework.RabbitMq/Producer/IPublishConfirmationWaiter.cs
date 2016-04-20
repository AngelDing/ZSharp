using System;
using System.Threading.Tasks;

namespace ZSharp.Framework.RabbitMq
{
    public interface IPublishConfirmationWaiter
    {
        void Wait(TimeSpan timeout);

        Task WaitAsync(TimeSpan timeout);

        void Cancel();
    }
}
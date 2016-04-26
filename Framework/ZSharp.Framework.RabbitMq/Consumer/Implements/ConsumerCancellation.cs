using System;

namespace ZSharp.Framework.RabbitMq
{
    public class ConsumerCancellation : IDisposable
    {
        private readonly Action onCancellation;

        public ConsumerCancellation(Action onCancellation)
        {
            this.onCancellation = onCancellation;
        }

        public void Dispose()
        {
            onCancellation();
        }
    }
}
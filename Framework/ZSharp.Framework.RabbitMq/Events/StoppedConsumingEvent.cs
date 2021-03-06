﻿namespace ZSharp.Framework.RabbitMq
{
    /// <summary>
    /// This event is fired when the logical consumer stops consuming.
    /// This is _not_ fired when a connection interruption causes RabbitMq to re-create
    /// a PersistentConsumer.
    /// </summary>
    public class StoppedConsumingEvent
    {
        public IConsumer Consumer { get; private set; }

        public StoppedConsumingEvent(IConsumer consumer)
        {
            Consumer = consumer;
        }
    }
}
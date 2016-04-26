﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ZSharp.Framework.RabbitMq
{
    public class PersistentConsumer : IConsumer
    {
        private readonly IQueue queue;
        private readonly Func<Byte[], MessageProperties, MessageReceivedInfo, Task> onMessage;
        private readonly IPersistentConnection connection;
        private readonly IConsumerConfiguration configuration;

        private readonly IInternalConsumerFactory internalConsumerFactory;
        private readonly IEventBus eventBus;

        private readonly ConcurrentDictionary<IInternalConsumer, object> internalConsumers = 
            new ConcurrentDictionary<IInternalConsumer, object>();

        private readonly IList<CancelSubscription> eventCancellations = new List<CancelSubscription>();

        public PersistentConsumer(
            IQueue queue, 
            Func<byte[], MessageProperties, MessageReceivedInfo, Task> onMessage, 
            IPersistentConnection connection, 
            IConsumerConfiguration configuration,
            IInternalConsumerFactory internalConsumerFactory,
            IEventBus eventBus)
        {
            this.queue = queue;
            this.onMessage = onMessage;
            this.connection = connection;
            this.configuration = configuration;
            this.internalConsumerFactory = internalConsumerFactory;
            this.eventBus = eventBus;
        }

        public IDisposable StartConsuming()
        {
            eventCancellations.Add(eventBus.Subscribe<ConnectionCreatedEvent>(e => ConnectionOnConnected()));
            eventCancellations.Add(eventBus.Subscribe<ConnectionDisconnectedEvent>(e => ConnectionOnDisconnected()));

            StartConsumingInternal();

            return new ConsumerCancellation(Dispose);
        }

        private void StartConsumingInternal()
        {
            if (disposed) return;

            if(!connection.IsConnected)
            {
                // connection is not connected, so just ignore this call. A consumer will
                // be created and start consuming when the connection reconnects.
                return;
            }

            var internalConsumer = internalConsumerFactory.CreateConsumer();
            internalConsumers.TryAdd(internalConsumer, null);

            internalConsumer.Cancelled += consumer => Dispose();

            internalConsumer.StartConsuming(
                connection, 
                queue,
                onMessage, 
                configuration
                );
        }

        private void ConnectionOnDisconnected()
        {
            internalConsumerFactory.OnDisconnected();
            internalConsumers.Clear();
        }

        private void ConnectionOnConnected()
        {
            StartConsumingInternal();
        }

        private bool disposed = false;

        public void Dispose()
        {
            if (disposed) return;

            disposed = true;

            eventBus.Publish(new StoppedConsumingEvent(this));

            foreach (var cancelSubscription in eventCancellations)
            {
                cancelSubscription();
            }

            foreach (var internalConsumer in internalConsumers.Keys)
            {
                internalConsumer.Dispose();
            }
        }
    }
}
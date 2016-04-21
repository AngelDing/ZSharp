using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Threading;
using ZSharp.Framework.Extensions;
using ZSharp.Framework.Utils;
using RabbitMQ.Client.Events;

namespace ZSharp.Framework.RabbitMq
{
    public class PersistentChannel : BaseRabbitMq, IPersistentChannel
    {
        private readonly IPersistentConnection connection;
        private IModel internalChannel;

        public PersistentChannel(IPersistentConnection connection)
        {
            this.connection = connection;
            WireUpEvents();
        }

        public void InvokeChannelAction(Action<IModel> channelAction)
        {
            GuardHelper.ArgumentNotNull(() => channelAction);
            var startTime = DateTime.UtcNow;
            var retryTimeout = TimeSpan.FromMilliseconds(50);
            while (!IsTimedOut(startTime))
            {
                try
                {
                    var channel = OpenChannel();
                    channelAction(channel);
                    return;
                }
                catch (OperationInterruptedException)
                {
                    CloseChannel();
                }
                catch (ZRabbitMqException)
                {
                    CloseChannel();
                }

                Thread.Sleep(retryTimeout);

                retryTimeout = retryTimeout.Double();
            }
            Logger.Error("Channel action timed out. Throwing exception to client.");
            throw new TimeoutException("The operation requested on PersistentChannel timed out.");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CloseChannel();
                Logger.Debug("Persistent internalChannel disposed.");
            }
        }

        private void WireUpEvents()
        {
            eventBus.Subscribe<ConnectionDisconnectedEvent>(OnConnectionDisconnected);
            eventBus.Subscribe<ConnectionCreatedEvent>(ConnectionOnConnected);
        }

        private void OnConnectionDisconnected(ConnectionDisconnectedEvent @event)
        {
            CloseChannel();
        }

        private void ConnectionOnConnected(ConnectionCreatedEvent @event)
        {
            try
            {
                OpenChannel();
            }
            catch (OperationInterruptedException)
            {
            }
            catch (ZRabbitMqException)
            {
            }
        }

        private IModel OpenChannel()
        {
            IModel channel;

            lock (this)
            {
                if (internalChannel != null)
                {
                    return internalChannel;
                }

                channel = connection.CreateModel();

                WireUpChannelEvents(channel);

                eventBus.Publish(new PublishChannelCreatedEvent(channel));

                internalChannel = channel;
            }

            Logger.Debug("Persistent channel connected.");
            return channel;
        }

        private void WireUpChannelEvents(IModel channel)
        {
            if (connectionConfiguration.PublisherConfirms)
            {
                channel.ConfirmSelect();
                channel.BasicAcks += OnAck;
                channel.BasicNacks += OnNack;
            }
            channel.BasicReturn += OnReturn;
        }

        private void OnReturn(object sender, BasicReturnEventArgs args)
        {
            eventBus.Publish(new ReturnedMessageEvent(args.Body,
                new MessageProperties(args.BasicProperties),
                new MessageReturnedInfo(args.Exchange, args.RoutingKey, args.ReplyText)));
        }

        private void OnAck(object sender, BasicAckEventArgs args)
        {
            eventBus.Publish(MessageConfirmationEvent.Ack((IModel)sender, args.DeliveryTag, args.Multiple));
        }

        private void OnNack(object sender, BasicNackEventArgs args)
        {
            eventBus.Publish(MessageConfirmationEvent.Nack((IModel)sender, args.DeliveryTag, args.Multiple));
        }

        private void CloseChannel()
        {
            lock (this)
            {
                if (internalChannel == null)
                {
                    return;
                }
                if (connectionConfiguration.PublisherConfirms)
                {
                    internalChannel.BasicAcks -= OnAck;
                    internalChannel.BasicNacks -= OnNack;
                }
                internalChannel.BasicReturn -= OnReturn;
                internalChannel = null;
            }

            Logger.Debug("Persistent channel disconnected.");
        }


        private bool IsTimedOut(DateTime startTime)
        {
            return !connectionConfiguration.Timeout.Equals(0) && startTime.AddSeconds(connectionConfiguration.Timeout) < DateTime.UtcNow;
        }
    }
}
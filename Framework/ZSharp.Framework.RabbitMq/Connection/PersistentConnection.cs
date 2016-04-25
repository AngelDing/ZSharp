using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace ZSharp.Framework.RabbitMq
{
    /// <summary>
    /// A connection that attempts to reconnect if the inner connection is closed.
    /// </summary>
    public class PersistentConnection : BaseRabbitMq, IPersistentConnection
    {
        private const int connectAttemptIntervalMilliseconds = 5000;

        private readonly IConnectionFactory connectionFactory;
        private readonly object locker = new object();
        private bool initialized = false;
        private IConnection connection;

        public PersistentConnection(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public void Initialize()
        {
            lock (locker)
            {
                if (initialized)
                {
                    throw new ZRabbitMqException("This PersistentConnection has already been initialized.");
                }
                initialized = true;
                TryToConnect(null);
            }
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new ZRabbitMqException("PersistentConnection: Attempt to create a channel while being disconnected.");
            }

            return connection.CreateModel();
        }

        public bool IsConnected
        {
            get { return connection != null && connection.IsOpen && !disposed; }
        }

        void StartTryToConnect()
        {
            var timer = new Timer(TryToConnect);
            timer.Change(connectAttemptIntervalMilliseconds, Timeout.Infinite);
        }

        void TryToConnect(object timer)
        {
            if (timer != null) ((Timer)timer).Dispose();

            Logger.Debug("Trying to connect");
            if (disposed) return;

            connectionFactory.Reset();
            do
            {
                try
                {
                    connection = connectionFactory.CreateConnection(); // A possible dispose race condition exists, whereby the Dispose() method may run while this loop is waiting on connectionFactory.CreateConnection() returning a connection.  In that case, a connection could be created and assigned to the connection variable, without it ever being later disposed, leading to app hang on shutdown.  The following if clause guards against this condition and ensures such connections are always disposed.
                    if (disposed)
                    {
                        connection.Dispose();
                        break;
                    }

                    connectionFactory.Success();

                }
                catch (SocketException socketException)
                {
                    LogException(socketException);
                }
                catch (BrokerUnreachableException brokerUnreachableException)
                {
                    LogException(brokerUnreachableException);
                }
            } while (!disposed && connectionFactory.Next());

            if (connectionFactory.Succeeded)
            {
                connection.ConnectionShutdown += OnConnectionShutdown;
                connection.ConnectionBlocked += OnConnectionBlocked;
                connection.ConnectionUnblocked += OnConnectionUnblocked;

                OnConnected();
                Logger.Info("Connected to RabbitMQ. Broker: '{0}', Port: {1}, VHost: '{2}'",
                    connectionFactory.CurrentHost.Ip,
                    connectionFactory.CurrentHost.Port,
                    connectionFactory.Configuration.VirtualHost);
            }
            else
            {
                if (!disposed)
                {
                    Logger.Error("Failed to connect to any Broker. Retrying in {0} ms\n",
                        connectAttemptIntervalMilliseconds);
                    StartTryToConnect();
                }
            }
        }

        void LogException(Exception exception)
        {
            Logger.Error("Failed to connect to Broker: '{0}', Port: {1} VHost: '{2}'. " +
                    "ExceptionMessage: '{3}'",
                connectionFactory.CurrentHost.Ip,
                connectionFactory.CurrentHost.Port,
                connectionFactory.Configuration.VirtualHost,
                exception.Message);
        }

        void OnConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            if (disposed) return;
            OnDisconnected();

            // try to reconnect and re-subscribe
            Logger.Info("Disconnected from RabbitMQ Broker");

            TryToConnect(null);
        }

        void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            Logger.Info("Connection blocked. Reason: '{0}'", e.Reason);

            EventBus.Publish(new ConnectionBlockedEvent(e.Reason));
        }

        void OnConnectionUnblocked(object sender, EventArgs e)
        {
            Logger.Info("Connection unblocked.");

            EventBus.Publish(new ConnectionUnblockedEvent());
        }

        public void OnConnected()
        {
            Logger.Debug("OnConnected event fired");
            EventBus.Publish(new ConnectionCreatedEvent());
        }

        public void OnDisconnected()
        {
            EventBus.Publish(new ConnectionDisconnectedEvent());
        }

        private bool disposed = false;
        public void Dispose()
        {
            if (disposed)
            {
                return;
            }
            disposed = true;
            if (connection != null)
            {
                try
                {
                    connection.Dispose();
                }
                catch (IOException exception)
                {
                    Logger.Debug(
                        "IOException thrown on connection dispose. Message: '{0}'. " +
                        "This is not normally a cause for concern.",
                        exception.Message);
                }
            }
        }
    }
}

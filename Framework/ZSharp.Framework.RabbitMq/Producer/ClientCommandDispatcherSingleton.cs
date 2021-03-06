﻿using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using ZSharp.Framework.Configurations;

namespace ZSharp.Framework.RabbitMq
{
    public class ClientCommandDispatcherSingleton : IClientCommandDispatcher
    {
        private const int queueSize = 1;
        private readonly CancellationTokenSource cancellation = new CancellationTokenSource();
        private readonly IPersistentChannel persistentChannel;
        private readonly BlockingCollection<Action> queue = new BlockingCollection<Action>(queueSize);

        public ClientCommandDispatcherSingleton(
            IRabbitMqConfiguration configuration,
            IPersistentConnection connection,
            IPersistentChannelFactory persistentChannelFactory)
        {
            persistentChannel = persistentChannelFactory.CreatePersistentChannel(connection);
            StartDispatcherThread(configuration);
        }

        public T Invoke<T>(Func<IModel, T> channelAction)
        {
            try
            {
                return InvokeAsync(channelAction).Result;
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }
        }

        public void Invoke(Action<IModel> channelAction)
        {
            try
            {
                InvokeAsync(channelAction).Wait();
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }
        }

        public Task<T> InvokeAsync<T>(Func<IModel, T> channelAction)
        {
            var tcs = new TaskCompletionSource<T>();
            try
            {
                queue.Add(() =>
                {
                    if (cancellation.IsCancellationRequested)
                    {
                        tcs.TrySetCanceledSafe();
                        return;
                    }
                    try
                    {
                        persistentChannel.InvokeChannelAction(channel => tcs.TrySetResultSafe(channelAction(channel)));
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetExceptionSafe(e);
                    }
                }, cancellation.Token);
            }
            catch (OperationCanceledException)
            {
                tcs.TrySetCanceled();
            }
            return tcs.Task;
        }

        public Task InvokeAsync(Action<IModel> channelAction)
        {
            return InvokeAsync(x =>
            {
                channelAction(x);
                return new NoContentStruct();
            });
        }

        public void Dispose()
        {
            cancellation.Cancel();
            persistentChannel.Dispose();
        }

        private void StartDispatcherThread(IRabbitMqConfiguration configuration)
        {
            var thread = new Thread(() =>
            {
                while (!cancellation.IsCancellationRequested)
                {
                    try
                    {
                        var channelAction = queue.Take(cancellation.Token);
                        channelAction();
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                }
            }) {Name = "Client Command Dispatcher Thread", IsBackground = configuration.UseBackgroundThreads};
            thread.Start();
        }

        private struct NoContentStruct
        {
        }
    }
}
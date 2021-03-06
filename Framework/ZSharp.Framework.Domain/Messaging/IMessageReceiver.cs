﻿using System;

namespace ZSharp.Framework.Domain
{   
    /// <summary>
    /// Abstracts the behavior of a receiving component that raises 
    /// an event for every received event.
    /// </summary>
    public interface IMessageReceiver : IDisposable
    {
        /// <summary>
        /// Event raised whenever a message is received. Consumer of 
        /// the event is responsible for disposing the message when 
        /// appropriate.
        /// </summary>
        event EventHandler<MessageReceivedEventArgs> OnMessageReceived;

        /// <summary>
        /// Starts the listener.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the listener.
        /// </summary>
        void Stop();
    }
}

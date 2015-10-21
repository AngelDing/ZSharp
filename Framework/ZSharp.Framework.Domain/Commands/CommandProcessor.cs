using System;
using System.Collections.Generic;
using System.Linq;
using ZSharp.Framework.Serializations;

namespace ZSharp.Framework.Domain
{
    /// <summary>
    /// Processes incoming commands from the bus and routes them to the appropriate 
    /// handlers.
    /// </summary>
    public class CommandProcessor : MessageProcessor, ICommandHandlerRegistry
    {
        private Dictionary<Type, ICommandHandler> handlers = new Dictionary<Type, ICommandHandler>();


        /// <summary>
        /// Initializes a new instance of the <see cref="CommandProcessor"/> class.
        /// </summary>
        /// <param name="receiver">The receiver to use. If the receiver is <see cref="IDisposable"/>, it will be disposed when the processor is 
        /// disposed.</param>
        /// <param name="serializer">The serializer to use for the message body.</param>
        public CommandProcessor(IMessageReceiver receiver, ISerializer serializer)
            : base(receiver, serializer)
        {
        }

        /// <summary>
        /// Registers the specified command handler.
        /// </summary>
        public void Register(ICommandHandler commandHandler)
        {
            var genericHandler = typeof(ICommandHandler<>);
            var supportedCommandTypes = commandHandler.GetType()
                .GetInterfaces()
                .Where(iface => iface.IsGenericType && iface.GetGenericTypeDefinition() == genericHandler)
                .Select(iface => iface.GetGenericArguments()[0])
                .ToList();

            if (handlers.Keys.Any(registeredType => supportedCommandTypes.Contains(registeredType)))
                throw new ArgumentException("The command handled by the received handler already has a registered handler.");

            // Register this handler for each of the handled types.
            foreach (var commandType in supportedCommandTypes)
            {
                this.handlers.Add(commandType, commandHandler);
            }
        }

        /// <summary>
        /// Processes the message by calling the registered handler.
        /// </summary>
        protected override void ProcessMessage(object payload, string correlationId)
        {
            var commandType = payload.GetType();
            ICommandHandler handler = null;

            if (this.handlers.TryGetValue(commandType, out handler))
            {
                Logger.Trace("-- Handled by " + handler.GetType().FullName);
                ((dynamic)handler).Handle((dynamic)payload);
            }

            // There can be a generic logging/tracing/auditing handlers
            if (this.handlers.TryGetValue(typeof(ICommand), out handler))
            {
                Logger.Trace("-- Handled by " + handler.GetType().FullName);
                ((dynamic)handler).Handle((dynamic)payload);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using ZSharp.Framework.Serializations;

namespace ZSharp.Framework.Domain
{
    public class CommandBus : ICommandBus
    {
        private readonly IMessageSender sender;
        private readonly ISerializer serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBus"/> class.
        /// </summary>
        /// <param name="serializer">The serializer to use for the message body.</param>
        public CommandBus(IMessageSender sender, ISerializer serializer)
        {
            this.sender = sender;
            this.serializer = serializer;
        }

        /// <summary>
        /// Sends the specified command.
        /// </summary>
        public void Send(Envelope<ICommand> command)
        {
            var message = BuildMessage(command);

            this.sender.Send(message);
        }

        public void Send(IEnumerable<Envelope<ICommand>> commands)
        {
            var messages = commands.Select(command => BuildMessage(command));

            this.sender.Send(messages);
        }

        private Message BuildMessage(Envelope<ICommand> command)
        {
            var typeFullName = command.Body.GetType().AssemblyQualifiedName;
            var payload = this.serializer.Serialize<string>(command.Body);
            return new Message(payload, typeFullName, command.Delay != TimeSpan.Zero ? (DateTime?)DateTime.UtcNow.Add(command.Delay) : null, command.CorrelationId);
        }
    }
}

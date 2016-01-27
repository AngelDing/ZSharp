using System.Collections.Generic;
using ZSharp.Framework.Extensions;
using System.Linq;

namespace ZSharp.Framework.Domain
{
    public class CommandBus : ICommandBus
    {
        private readonly IMessageSender sender;

        public CommandBus(IMessageSender sender)
        {
            this.sender = sender;
        }

        public void Send(Envelope<ICommand> command)
        {
            InitCommand(command);
            this.sender.Send(command);
        }      

        public void Send(IEnumerable<Envelope<ICommand>> commands)
        {
            InitCommands(commands);
            this.sender.Send(commands);
        }

        private void InitCommands(IEnumerable<Envelope<ICommand>> commands)
        {
            foreach (var c in commands)
            {
                InitCommand(c);
            }
        }

        private void InitCommand(Envelope<ICommand> command)
        {
            if (command.Topic.IsNullOrEmpty())
            {
                command.Topic = Constants.ApplicationRuntime.DefaultCommandTopic;
            }
        }
    }

    /// <summary>
    /// 客户端应该定义一套自己的Topic（队列，路由等）
    /// </summary>
    public static class CommandBusExtensions
    {
        public static void Send(this ICommandBus bus, ICommand command, string topic)
        {
            bus.Send(CreateEnvelopeCommand(command, topic));
        }

        public static void Send(this ICommandBus bus, IEnumerable<ICommand> commands, string topic)
        {
            bus.Send(commands.Select(x => CreateEnvelopeCommand(x, topic)));
        }

        private static Envelope<ICommand> CreateEnvelopeCommand(ICommand command, string topic)
        {
            var envelopeCommand = new Envelope<ICommand>(command);
            envelopeCommand.Topic = topic;
            return envelopeCommand;
        }
    }
}

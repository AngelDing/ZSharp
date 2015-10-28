using System.Collections.Generic;
using System.Linq;
using ZSharp.Framework.Configurations;
using ZSharp.Framework.Extensions;

namespace ZSharp.Framework.Domain
{   
    public static class CommandBusExtensions
    {
        public static void Send(this ICommandBus bus, ICommand command, string topic = null, string sysCode = null)
        {
            bus.Send(CreateEnvelopeCommand(command, sysCode, topic));
        }

        public static void Send(this ICommandBus bus, IEnumerable<ICommand> commands, string topic = null, string sysCode = null)
        {
            bus.Send(commands.Select(x => CreateEnvelopeCommand(x, sysCode, topic)));
        }

        private static Envelope<ICommand> CreateEnvelopeCommand(ICommand command, string topic, string sysCode)
        {
            if (sysCode.IsNullOrEmpty())
            {
                sysCode = CommonConfig.SystemCode;
            }
            if (topic.IsNullOrEmpty())
            {
                topic = Constants.ApplicationRuntime.DefaultTopic;
            }
            var envelopeCommand = new Envelope<ICommand>(command);
            envelopeCommand.SysCode = sysCode;
            envelopeCommand.Topic = topic;

            return envelopeCommand;
        }
    }
}

using System;
using ZSharp.Framework.Domain;

namespace Framework.Test.Core
{
    public class TestHandler :
        IEventHandler<EventTestMsg>,
        ICommandHandler<CommandTestMsg>
    {
        public void Handle(CommandTestMsg message)
        {
            Console.WriteLine(message.Name);
        }

        public void Handle(EventTestMsg message)
        {
            Console.WriteLine(message.Name);
        }
    }
}

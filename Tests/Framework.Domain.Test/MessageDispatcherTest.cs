using Xunit;
using ZSharp.Framework.Domain;
using Framework.Test.Core;
using System;

namespace Framework.Domain.Test
{
    public class MessageDispatcherTest
    {
        private readonly MessageDispatcher dispatcher;
        public MessageDispatcherTest()
        {
            dispatcher = new MessageDispatcher();
            dispatcher.Register(new CommonHandler());
            dispatcher.Register(new TestHandler());
        }

        [Fact]
        public void send_commond_msg_test()
        {
            var msg = new CommandTestMsg { Name = "Command" };
            dispatcher.DispatchMessage(msg, Guid.NewGuid());
        }
    }
}

using System;
using ZSharp.Framework.Domain;

namespace Framework.Test.Core
{
    public class CommonHandler : IHandler<IMessage>
    {
        public void Handle(IMessage message)
        {
            Console.WriteLine(message.GetType().ToString());
        }
    }
}

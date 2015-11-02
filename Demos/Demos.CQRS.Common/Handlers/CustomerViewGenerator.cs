using System;
using ZSharp.Framework.Domain;

namespace Demos.CQRS.Common
{
    public class CustomerViewGenerator :
        IEventHandler<CustomerCreatedEvent>,
        IEventHandler<NameChangedEvent>,
        IEventHandler<EmailChangedEvent>
    {
        public void Handle(EmailChangedEvent message)
        {
            Console.WriteLine("Change Email To : " + message.Email);
        }

        public void Handle(CustomerCreatedEvent message)
        {
            Console.WriteLine("Generate Customer View Model : " + message.FirstName + message.LastName);
        }

        public void Handle(NameChangedEvent message)
        {
            Console.WriteLine("Change Customer Name To : " + message.FirstName + message.LastName);
        }
    }
}

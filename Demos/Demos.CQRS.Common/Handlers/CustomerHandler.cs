using System;
using ZSharp.Framework.Domain;

namespace Demos.CQRS.Common
{
    public class CustomerHandler :
        ICommandHandler<CreateCustomerCommand>,
        IEventHandler<ChangeCustomerNameEvent>,
        IEventHandler<ChangeEmailEvent>
    {
        public void Handle(ChangeEmailEvent message)
        {
            Console.WriteLine("Change Email To : " + message.Email);
        }

        public void Handle(ChangeCustomerNameEvent message)
        {
            Console.WriteLine("Change Customer Name To : " + message.FirstName + message.LastName);
        }

        public void Handle(CreateCustomerCommand message)
        {
            Console.WriteLine("Create Custome : " + message.FirstName);
        }
    }
}

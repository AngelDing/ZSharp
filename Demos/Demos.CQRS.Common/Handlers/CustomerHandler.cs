using System;
using ZSharp.Framework.Domain;
using ZSharp.Framework.Infrastructure;

namespace Demos.CQRS.Common
{
    public class CustomerHandler :
        ICommandHandler<CreateCustomerCommand>        
    {       
        public void Handle(CreateCustomerCommand message)
        {
            var info = message.Map<CustomerInfo>();
            new Customer(info).Create();
            Console.WriteLine("Create Custome : " + message.FirstName);
        }
    }
}

using System;
using ZSharp.Framework.Domain;

namespace Demos.CQRS.Common
{
    public class Customer
    {
        private readonly ICustomerRepository customerRepo;

        public Customer(ICustomerRepository customerRepo)
        {
            this.customerRepo = customerRepo;
        }

        public CustomerEntity Entity { get; set; }
    }
}

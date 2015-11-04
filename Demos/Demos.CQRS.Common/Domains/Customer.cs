using System;
using ZSharp.Framework.Domain;
using ZSharp.Framework.Infrastructure;

namespace Demos.CQRS.Common
{
    public class Customer
    {
        private CustomerEntity entity;
        private readonly ICustomerRepository customerRepo;
        private readonly IEventBus eventBus;

        public Customer(int id) : this()
        {
            this.entity = customerRepo.GetByKey(id);            
        }

        public Customer(CustomerInfo cInfo) : this()
        {
            this.entity = cInfo.Map<CustomerEntity>();
        }

        public Customer()
        {
            this.customerRepo = ServiceLocator.GetInstance<ICustomerRepository>();
            this.eventBus = ServiceLocator.GetInstance<IEventBus>();
        }

        public void Create()
        {
            entity.CreationTime = DateTimeOffset.Now;
            entity.CreatedBy = "Jacky";
            customerRepo.Insert(entity);
            var @event = entity.Map<CustomerCreatedEvent>();
            eventBus.Publish(@event);
        }
    }
}

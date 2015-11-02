namespace Demos.CQRS.Common
{
    public class CustomerService : ICustomerService
    {
        public void CreateCustomer(CustomerInfo cInfo)
        {
            new Customer(cInfo).Create();
        }
    }
}

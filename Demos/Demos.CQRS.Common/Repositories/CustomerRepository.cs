
using ZSharp.Framework.Repositories;
using ZSharp.Framework.SqlDb;

namespace Demos.CQRS.Common
{
    public class CustomerRepository : EfRepository<CustomerEntity, int>, ICustomerRepository
    {
        public CustomerRepository(IRepositoryContext context)
            : base(context)
        {
        }
    }
}

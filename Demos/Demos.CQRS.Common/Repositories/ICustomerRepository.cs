using ZSharp.Framework.Repositories;

namespace Demos.CQRS.Common
{
    public interface ICustomerRepository : IRepository<CustomerEntity, int>
    {
    }
}
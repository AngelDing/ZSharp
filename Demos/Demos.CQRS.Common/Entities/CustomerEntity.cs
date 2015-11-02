
using ZSharp.Framework.Entities;
using ZSharp.Framework.SqlDb;

namespace Demos.CQRS.Common
{
    public class CustomerEntity : EfEntity<int>, IAggregateRoot<int>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }
}

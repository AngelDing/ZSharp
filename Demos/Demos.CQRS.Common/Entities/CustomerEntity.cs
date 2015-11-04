using ZSharp.Framework.Entities;

namespace Demos.CQRS.Common
{
    public class CustomerEntity : FullAuditedEntity<int, string>, IAggregateRoot<int>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }
}


using System;
using ZSharp.Framework.Entities;
using ZSharp.Framework.SqlDb;

namespace Demos.CQRS.Common
{
    public class CustomerEntity : EfEntity<int>, IAggregateRoot<int>, IBusinessEntity<int, string>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTimeOffset? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public bool IsDeleted { get; set; }
    }
}

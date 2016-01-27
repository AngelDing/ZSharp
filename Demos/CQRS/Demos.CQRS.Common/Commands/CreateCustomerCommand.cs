using ZSharp.Framework.Domain;

namespace Demos.CQRS.Common
{
    public class CreateCustomerCommand : Command
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }
}

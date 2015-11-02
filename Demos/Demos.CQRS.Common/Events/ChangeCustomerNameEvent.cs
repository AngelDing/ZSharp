using ZSharp.Framework.Domain;

namespace Demos.CQRS.Common
{
    public class ChangeCustomerNameEvent : Event
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}

using ZSharp.Framework.Domain;

namespace Demos.CQRS.Common
{
    public class NameChangedEvent : Event
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}

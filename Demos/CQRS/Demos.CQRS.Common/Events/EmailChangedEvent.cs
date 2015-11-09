using ZSharp.Framework.Domain;

namespace Demos.CQRS.Common
{
    public class EmailChangedEvent : Event
    {
        public string Email { get; set; }
    }
}

using ZSharp.Framework.Domain;

namespace Demos.CQRS.Common
{
    public class ChangeEmailEvent : Event
    {
        public string Email { get; set; }
    }
}

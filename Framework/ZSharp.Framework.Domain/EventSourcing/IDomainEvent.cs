
namespace ZSharp.Framework.Domain
{
    public interface IDomainEvent : IEvent
    {
        int Version { get; }

        string CorrelationId { get; }
    }
}

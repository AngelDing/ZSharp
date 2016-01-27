
namespace ZSharp.Framework.Domain
{
    public interface ISnapshotPolicy
    {
        bool ShouldCreateSnapshot(IEventSourced eventSourced);
    }
}

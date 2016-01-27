
namespace ZSharp.Framework.Domain
{
    public class NoSnapshotPolicy : ISnapshotPolicy
    {
        public bool ShouldCreateSnapshot(IEventSourced eventSourced)
        {
            return false;
        }
    }
}

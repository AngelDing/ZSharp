
namespace ZSharp.Framework.Domain
{
    public interface ISnapshotOrignator
    {
        void LoadFromSnapshot(ISnapshot snapshot);

        ISnapshot CreateSnapshot();
    }
}

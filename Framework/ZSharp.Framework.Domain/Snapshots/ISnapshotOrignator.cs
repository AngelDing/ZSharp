
namespace ZSharp.Framework.Domain
{
    public interface ISnapshotOrignator
    {
        void BuildFromSnapshot(ISnapshot snapshot);

        ISnapshot CreateSnapshot();
    }
}

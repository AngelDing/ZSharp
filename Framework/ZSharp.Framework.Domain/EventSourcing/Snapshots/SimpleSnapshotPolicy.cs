namespace ZSharp.Framework.Domain
{
    public class SimpleSnapshotPolicy : ISnapshotPolicy
    {
        private int snapshotIntervalInEvents;

        public SimpleSnapshotPolicy(int snapshotIntervalInEvents)
        {
            this.snapshotIntervalInEvents = snapshotIntervalInEvents;
        }

        public bool ShouldCreateSnapshot(IEventSourced eventSourced)
        {
            var isShould = false;

            if ((eventSourced.Version + 1) % snapshotIntervalInEvents == 0)
            {
                isShould = true;
            }

            return isShould;
        }
    }
}
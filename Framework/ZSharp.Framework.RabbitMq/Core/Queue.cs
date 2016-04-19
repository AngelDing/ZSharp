using ZSharp.Framework.Utils;

namespace ZSharp.Framework.RabbitMq
{
    public class Queue : IQueue
    {
        public Queue(string name, bool isExclusive)
        {
            GuardHelper.ArgumentNotNull(() => name);
            Name = name;
            IsExclusive = isExclusive;
        }

        public string Name { get; private set; }

        public bool IsExclusive { get; private set; }
    }
}
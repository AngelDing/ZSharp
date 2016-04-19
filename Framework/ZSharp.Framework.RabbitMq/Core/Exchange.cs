using ZSharp.Framework.Utils;

namespace ZSharp.Framework.RabbitMq
{
    public class Exchange : IExchange
    {
        private static readonly Exchange defaultExchange = new Exchange("");

        public string Name { get; private set; }

        public static IExchange GetDefault()
        {
            return defaultExchange;
        }

        public Exchange(string name)
        {
            GuardHelper.ArgumentNotNull(() => name);
            Name = name;
        }
    }
}
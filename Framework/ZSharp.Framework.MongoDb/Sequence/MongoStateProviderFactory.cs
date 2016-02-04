using ZSharp.Framework.Sequence;

namespace ZSharp.Framework.MongoDb
{
    public class MongoStateProviderFactory
    {
        public static IStateProvider Get(string nameOrConnectionStr)
        {
            return new MongoStateProvider(nameOrConnectionStr);
        }
    }
}

using ZSharp.Framework.Sequence;

namespace ZSharp.Framework.SqlDb
{
    public class SqlServerStateProviderFactory
    {
        public static IStateProvider Get(string nameOrConnectionStr)
        {
            return new SqlServerStateProvider(nameOrConnectionStr);
        }
    }
}

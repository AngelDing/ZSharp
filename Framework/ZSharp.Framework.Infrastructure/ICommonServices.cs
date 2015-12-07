using ZSharp.Framework.Caching;
using ZSharp.Framework.Configurations;
using ZSharp.Framework.Logging;
using ZSharp.Framework.Repositories;

namespace ZSharp.Framework.Infrastructure
{
    public interface ICommonServices
    {
        IRepositoryContext RepoContext { get; }

        ICacheManager Cache { get; }

        ILogger Logger { get; }

        ISettingService Settings { get; }
    }
}

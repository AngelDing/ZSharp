
namespace ZSharp.Framework.Configurations
{
    public interface IRedisConfiguration
    {
        string Name { get; }

        RedisHostCollection RedisHosts { get; }

        bool AllowAdmin { get; }

        bool Ssl { get; }

        int ConnectTimeout { get; }

        int Database { get; }
    }
}

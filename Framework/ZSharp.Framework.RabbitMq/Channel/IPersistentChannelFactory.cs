using ZSharp.Framework.Utils;
namespace ZSharp.Framework.RabbitMq
{
    public interface IPersistentChannelFactory
    {
        IPersistentChannel CreatePersistentChannel(IPersistentConnection connection);
    }

    public class PersistentChannelFactory : IPersistentChannelFactory
    {
        public IPersistentChannel CreatePersistentChannel(IPersistentConnection connection)
        {
            GuardHelper.ArgumentNotNull(() => connection);
            return new PersistentChannel(connection);
        }
    }
}
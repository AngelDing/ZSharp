
namespace ZSharp.Framework.Domain
{
    public class NullMessageLogRepository : IMessageLogRepository
    {
        public void Save(IMessage msg)
        {
        }
    }
}

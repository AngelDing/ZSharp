
namespace ZSharp.Framework.Domain
{
    public interface IMessageLogRepository
    {
        void Save(IMessage msg);
    }
}

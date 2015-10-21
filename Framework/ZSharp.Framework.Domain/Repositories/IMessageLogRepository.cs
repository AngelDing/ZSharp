
namespace ZSharp.Framework.Domain
{
    public interface IMessageLogRepository
    {
        void Save(IEvent @event);

        void Save(ICommand command);
    }
}

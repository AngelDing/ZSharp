namespace ZSharp.Framework.Domain
{

    /// <summary>
    /// The event log runs directly in-proc and is implemented as an event
    /// and command handler instead of a raw message listener.
    /// </summary>
    public class MessageLogHandler : IEventHandler<IEvent>, ICommandHandler<ICommand>
    {
        private readonly IMessageLogRepository logRepo;

        public MessageLogHandler(IMessageLogRepository logRepo)
        {
            this.logRepo = logRepo;
        }

        public void Handle(IEvent @event)
        {
            this.logRepo.Save(@event);
        }

        public void Handle(ICommand command)
        {
            this.logRepo.Save(command);
        }
    }
}

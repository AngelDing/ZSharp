namespace ZSharp.Framework.Domain
{
    public class MessageLogHandler : IHandler<IMessage>
    {
        private readonly IMessageLogRepository logRepo;

        public MessageLogHandler(IMessageLogRepository logRepo)
        {
            this.logRepo = logRepo;
        }

        public void Handle(IMessage msg)
        {
            this.logRepo.Save(msg);
        }
    }
}

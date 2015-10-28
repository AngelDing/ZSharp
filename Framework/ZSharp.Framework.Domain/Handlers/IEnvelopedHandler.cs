namespace ZSharp.Framework.Domain
{
    public interface IEnvelopedHandler<T> : IHandler where T : IMessage
    {
        void Handle(Envelope<T> envelope);
    }
}
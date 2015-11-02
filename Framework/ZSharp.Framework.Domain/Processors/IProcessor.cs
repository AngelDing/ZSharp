namespace ZSharp.Framework.Domain
{
    public interface IProcessor : IHandlerRegistry
    {
        void Start();

        void Stop();
    }
}

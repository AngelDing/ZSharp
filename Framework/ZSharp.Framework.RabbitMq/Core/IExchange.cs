namespace ZSharp.Framework.RabbitMq
{
    public interface IExchange : IBindable
    {
        string Name { get; }
    }
}
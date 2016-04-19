namespace ZSharp.Framework.RabbitMq
{
    public interface IBinding
    {
        IBindable Bindable { get; }

        IExchange Exchange { get; }

        string RoutingKey { get; }
    }
}
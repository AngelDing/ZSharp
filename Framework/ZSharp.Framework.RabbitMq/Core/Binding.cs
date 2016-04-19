using ZSharp.Framework.Utils;

namespace ZSharp.Framework.RabbitMq
{
    public class Binding : IBinding
    {
        public Binding(IBindable bindable, IExchange exchange, string routingKey)
        {
            GuardHelper.ArgumentNotNull(() => bindable);
            GuardHelper.ArgumentNotNull(() => exchange);
            GuardHelper.ArgumentNotNull(() => routingKey);

            Bindable = bindable;
            Exchange = exchange;
            RoutingKey = routingKey;
        }

        public IBindable Bindable { get; private set; }

        public IExchange Exchange { get; private set; }

        public string RoutingKey { get; private set; }
    }
}
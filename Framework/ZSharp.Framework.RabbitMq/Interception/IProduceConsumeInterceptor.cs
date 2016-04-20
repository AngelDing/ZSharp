namespace ZSharp.Framework.RabbitMq
{
    public interface IProduceConsumeInterceptor
    {
        RawMessage OnProduce(RawMessage rawMessage);

        RawMessage OnConsume(RawMessage rawMessage);
    }
}
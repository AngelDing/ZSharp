﻿namespace ZSharp.Framework.RabbitMq
{
    public class DefaultInterceptor : IProduceConsumeInterceptor
    {
        public RawMessage OnProduce(RawMessage rawMessage)
        {
            return rawMessage;
        }

        public RawMessage OnConsume(RawMessage rawMessage)
        {
            return rawMessage;
        }
    }
}
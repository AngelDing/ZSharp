using System;

namespace ZSharp.Framework.RabbitMq
{
    public interface ICorrelationIdGenerationStrategy
    {
        string GetCorrelationId();
    }

    public class DefaultCorrelationIdGenerationStrategy : ICorrelationIdGenerationStrategy
    {
        public string GetCorrelationId()
        {
            return Guid.NewGuid().ToString();
        } 
    }
}

using System;

namespace ZSharp.Framework.Domain
{
    /// <summary>
    /// 表示一个领域事件消息，属于有序事件流，用于事件溯源
    /// </summary>
    public interface IDomainEvent : IEvent
    {
        /// <summary>
        /// 领域事件版本
        /// </summary>
        int Version { get; }

        /// <summary>
        /// 引发此事件的源命令/事件，一般指源命令Id
        /// </summary>
        Guid CorrelationId { get; set; }
    }

    public abstract class DomainEvent : Event, IDomainEvent
    {        
        public Guid CorrelationId { get; set; }

        public int Version { get; set; }
    }
}
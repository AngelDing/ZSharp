using ZSharp.Framework.Entities;
using System;
using System.Collections.Generic;

namespace ZSharp.Framework.Domain
{   
    /// <summary>
    /// 事件溯源接口
    /// </summary>
    public interface IEventSourced : ISnapshotOrignator, IAggregateRoot
    {
        /// <summary>
        /// 领域实体Id
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// 得到领域实体的版本，实体被更新和事件产生时,增加版本
        /// </summary>
        int Version { get; }

        /// <summary>
        /// 与此版本关联的源命令或者事件的消息Id
        /// </summary>
        Guid CorrelationId { get; }

        /// <summary>
        /// 实体产生领域事件的主题，消息接受者关注主题，获取消息并交给相应Handler处理
        /// 未设置则采用默认的事件主题
        /// </summary>
        string Topic { get; }

        /// <summary>
        /// 暂未处理的事件
        /// </summary>
        IEnumerable<IDomainEvent> PendingEvents { get; }

        /// <summary>
        /// 加载历史事件，还原领域实体
        /// </summary>
        /// <param name="historyEvents"></param>
        void LoadFromHistory(IEnumerable<IDomainEvent> historyEvents);
    }
}

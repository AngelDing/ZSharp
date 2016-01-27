using System;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.Domain
{
    public interface IMessage
    {
        /// <summary>
        /// 消息唯一标识符：
        /// 1.对Command来说是就是CommandId；
        /// 2.对Event来说，没有实际用处，Event需要关注的是SourceId
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// 消息创建人
        /// </summary>
        string CreatedBy { get; }

        /// <summary>
        /// 消息创建时间
        /// </summary>
        DateTimeOffset CreationTime { get; }
    }

    public interface ICommand : IMessage
    {
    }

    public interface IEvent : IMessage
    {
        /// <summary>
        /// 原始事件源的标识符，如：AggregateId
        /// </summary>
        Guid SourceId { get; set; }
    }

    public abstract class Message : IMessage
    {
        public Message()
        {
            this.Id = GuidHelper.NewSequentialId();
            CreatedBy = Constants.ApplicationRuntime.DefaultUserName;
            this.CreationTime = DateTimeOffset.Now;
        }

        public Guid Id { get; private set; }

        public string CreatedBy { get; set; }

        public DateTimeOffset CreationTime { get; private set; }
    }

    public class Command : Message, ICommand
    {
    }

    public class Event : Message, IEvent
    {
        public Guid SourceId { get; set; }
    }
}

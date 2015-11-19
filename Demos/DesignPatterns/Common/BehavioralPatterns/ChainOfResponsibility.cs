using System;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// 职责链 ： 使多个对象都有机会处理请求，从而避免请求的发送者和接收者之间的耦合关系。
///         将这些对象连成一条链，并沿着这条链传递该请求，直到有一个对象能处理它。
/// 适用场景 ：
///         有多个对象可以处理一个请求，哪个对象处理该请求运行时自动确定。
///         你想在不明确指定接收者的情况下，向多个对象中的一个提交一个请求。
///         可处理一个请求的对象集合应被动态指定。
/// 效果 ：
///         降低耦合度。对象无需知道哪个一个对象处理其请求，仅需知道对象被处理。
///         增强了给对象指派职责的灵活性。可以运行时对该链进行动态增加或修改。
/// 相关模式 ：
///         Chain of Resposibility 常与 Composite 一起使用。一个构件的父构件可作为它的后继。
/// </summary>
namespace Common.BehavioralPatterns
{
    public interface IRequest
    {
        object EnumType { get; set; }
    }

    /// <summary>
    /// 抽象的操作对象
    /// </summary>
    public interface IHandler
    {
        /// <summary>
        /// 处理客户程序请求
        /// </summary>
        /// <param name="request"></param>
        void Handle(IRequest request);

        /// <summary>
        /// 后继结点
        /// </summary>
        IList<IHandler> Successors { get; set; }

        /// <summary>
        /// 当前Handler处理的请求类型
        /// </summary>
        object EnumType { get; set; }

        /// <summary>
        /// 是否设置当前IHandler对象的处理为断点
        /// </summary>
        bool HasBreakPoint { set; get; }

        /// <summary>
        /// 当前Handler对象的断点处理事件
        /// </summary>
        event EventHandler<CallHandlerEventArgs> Break;
    }

    /// <summary>
    /// 响应断点的事件参数
    /// </summary>
    public class CallHandlerEventArgs : EventArgs
    {
        public IHandler Handler { get; private set; }

        public IRequest Request { get; private set; }

        public CallHandlerEventArgs(IHandler handler, IRequest request)
        {
            this.Handler = handler;
            this.Request = request;
        }
    }

    /// <summary>
    /// 操作的抽象类型
    /// </summary>
    public abstract class BaseHandler : IHandler
    {
        private object enumType;

        public BaseHandler(object enumType)
           : this(enumType, new List<IHandler>())
        {
            this.EnumType = enumType;
        }

        public BaseHandler(object enumType, IHandler successor)
            : this(enumType, new List<IHandler> { successor})
        {          
        }

        public BaseHandler(object enumType, IList<IHandler> successors)
        {
            this.EnumType = enumType;
            this.Successors = successors;
        }

        public IList<IHandler> Successors { get; set; }

        public object EnumType
        {
            get
            {
                return enumType;
            }
            set
            {
                CheckIsEnum(value);
                enumType = value;
            }
        }

        /// <summary>
        /// 需要具体IHandler类型处理的内容
        /// </summary>
        /// <param name="request"></param>
        public abstract void Process(IRequest request);

        /// <summary>
        /// 按照链式方式依次把调用继续下去
        /// </summary>
        /// <param name="request"></param>
        public virtual void Handle(IRequest request)
        {
            // 如果发现当前操作设置了断点，则抛出事件
            if (HasBreakPoint)
            {
                OnBreak(new CallHandlerEventArgs(this, request));
            }

            if (request == null)
            {
                return;
            }
            if (request.EnumType.GetHashCode() == EnumType.GetHashCode())
            {
                Process(request);
            }
            else
            {
                if (Successors.Any())
                {
                    foreach (var s in Successors)
                    {
                        s.Handle(request);
                    }
                }
            }
        }

        /// <summary>
        /// 执行过程中动态提示TraceManager对当前断点响应
        /// </summary>
        public bool HasBreakPoint { get; set; }

        /// <summary>
        /// 断点事件响应
        /// </summary>
        public event EventHandler<CallHandlerEventArgs> Break;

        public virtual void OnBreak(CallHandlerEventArgs args)
        {
            if (Break != null)
            {
                Break(this, args);
            }
        }

        private void CheckIsEnum(object enumType)
        {
            var type = enumType.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("enumType類型必須為枚舉！");
            }
        }
    }
}

using System;
using System.Collections.Generic;

/// <summary>
/// 中介者：
///     用一个中介对象来封装一系列的对象交互;中介者使各对象不需要显式地相互引用，
///     从而使其耦合松散，而且可以独立地改变它们之间的交互。
/// 适用性：
///     一组对象定义良好但是使用复杂的通信方式。产生的相互依赖关系结构混乱且难以理解。
///     一个对象引用其他很多对象并且直接与这些对象通信，导致难以复用该对象。
///     想定制一个分布在多个类中的行为，而又不想生成太多的子类。
/// </summary>

namespace Common.BehavioralPatterns
{
    #region Classic
    /// <summary>
    /// 协同对象接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IColleague<T>
    {
        T Data { get; set; }

        IMediator<T> Mediator { get; set; }
    }

    public abstract class ColleagueBase<T> : IColleague<T>
    {
        protected T data;
        protected IMediator<T> mediator;

        public virtual T Data
        {
            get { return data; }
            set { data = value; }
        }

        public virtual IMediator<T> Mediator
        {
            get { return mediator; }
            set { mediator = value; }
        }
    }

    /// <summary>
    /// 抽象中介者接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMediator<T>
    {
        /// <summary>
        /// 提供给IColleague的触发方法
        /// </summary>
        void Change();

        /// <summary>
        /// 建立协作关系的方法
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="consumers"></param>
        void Introduce(IColleague<T> provider, IList<IColleague<T>> consumers);

        void Introduce(IColleague<T> provider, params IColleague<T>[] consumers);
    }

    /// <summary>
    /// 具体中介者类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Mediator<T> : IMediator<T>
    {
        protected IColleague<T> provider = null;
        protected IList<IColleague<T>> consumers = null;

        public virtual void Change()
        {
            if ((provider != null) && (consumers != null))
            {
                foreach (IColleague<T> colleague in consumers)
                {
                    colleague.Data = provider.Data;
                }
            }
        }

        public virtual void Introduce(IColleague<T> provider, IList<IColleague<T>> consumers)
        {
            this.provider = provider;
            this.consumers = consumers;
        }

        public virtual void Introduce(IColleague<T> provider, params IColleague<T>[] consumers)
        {
            if (consumers.Length > 0)
            {
                this.consumers = new List<IColleague<T>>(consumers);
            }

            this.provider = provider;
        }                 
    }
    #endregion

    #region Classic Client Test
    /// <summary>
    /// provider
    /// </summary>
    class A : ColleagueBase<int>
    {
        public override int Data
        {
            get { return base.Data; }
            set
            {
                base.Data = value;
                mediator.Change();
            }
        }
    }
    /// <summary>
    /// consumer
    /// </summary>
    public class B : ColleagueBase<int> { }

    public class C : ColleagueBase<int> { }

    public class MediatorClient
    {
        public void Test()
        {
            // 验证Mediator对协作对象间的通知作用
            Mediator<int> mA2BC = new Mediator<int>();
            A a = new A();
            B b = new B();
            C c = new C();
            a.Mediator = mA2BC;
            b.Mediator = mA2BC;
            c.Mediator = mA2BC;
            mA2BC.Introduce(a, b, c);
            a.Data = 20;
            Console.WriteLine(b.Data);
            Console.WriteLine(c.Data);

            // 更改协作关系
            Mediator<int> mA2B = new Mediator<int>();
            a.Mediator = mA2B;
            b.Mediator = mA2B;
            c.Mediator = mA2B;
            mA2B.Introduce(a, b);
            a.Data = 30;
            Console.WriteLine(b.Data);
            // C 因为不在新的协作关系之内，所以不变化
            Console.WriteLine(c.Data);
        }
    }

    #endregion

    #region Delegating
    public class DataEventArgs<T> : EventArgs
    {
        public T Data;
        public DataEventArgs(T data) { this.Data = data; }
    }

    public abstract class BaseColleague<T>
    {
        public event EventHandler<DataEventArgs<T>> Changed;

        protected T data;
        public virtual T Data
        {
            get { return data; }
            set { data = value; }
        }

        public virtual void OnChanged(object sender, DataEventArgs<T> args)
        {
            if (Changed != null)
            {
                Changed(this, args);
            }
        }

        public virtual void Received(object sender, DataEventArgs<T> args)
        {
            Data = args.Data;
        }
    }

    public class AA: BaseColleague<int>
    {
        public override int Data
        {
            get { return base.Data; }
            set
            {
                base.Data = value;
                /// 把消息抛给作为中介的.NET事件机制
                OnChanged(this, new DataEventArgs<int>(value));
            }
        }
    }
    public class BB : BaseColleague<int> { }

    public class CC : BaseColleague<int> { }

    public class MerdiatorDelegatingClient
    {
        public void Test()
        {
            // 验证.NET事件对协作对象间的通知作用
            // 其中.NET事件机制作为隐含的Mediator对象出现
            var a = new AA();
            var b = new BB();
            var c = new CC();
            a.Changed += b.Received;
            a.Changed += c.Received;
            a.Data = 20;
            Console.WriteLine(b.Data);
            Console.WriteLine(c.Data);

            // 更改协作关系
            a.Changed -= c.Received;
            a.Data = 30;
            Console.WriteLine(b.Data);
            // C 因为不在新的协作关系之内，所以不变化
            Console.WriteLine(c.Data);
        }
    }
    #endregion
}

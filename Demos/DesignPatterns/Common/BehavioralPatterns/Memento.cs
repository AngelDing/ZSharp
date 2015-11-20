using System;

/// <summary>
/// 备忘录模式又叫做快照模式(Snapshot Pattern)或Token模式，是对象的行为模式。
/// 备忘录对象是一个用来存储另外一个对象内部状态的快照的对象。备忘录模式的用意是在不破坏封装的条件下，
/// 将一个对象的状态捕捉(Capture)住，并外部化，存储起来，从而可以在将来合适的时候把这个对象还原到存储起来的状态。
/// 备忘录模式常常与命令模式和迭代子模式一同使用。
/// </summary>

namespace Common.BehavioralPatterns
{
    /// <summary>
    /// 为了便于定义抽象状态类型所定义的接口
    /// </summary>
    public interface ISnapshoot { }

    /// <summary>
    /// 抽象备忘录对象接口
    /// </summary>
    public interface IMemento<T> where T : ISnapshoot
    {
        T GetSnapshoot();

        void SetSnapshoot(T snapshoot);
    }

    /// <summary>
    /// 快照可以存在内存中，也可以持久化到db中，
    /// 如果存入db中，则可以叫做IMementoStore，或者IMementoRepository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MementoBase<T> : IMemento<T>
        where T : ISnapshoot
    {
        private T snapshoot;

        public virtual T GetSnapshoot()
        {
            return this.snapshoot;
        }

        public virtual void SetSnapshoot(T snapshoot)
        {
            this.snapshoot = snapshoot;
        }
    }

    /// <summary>
    /// 抽象的发起人对象接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// /// <typeparam name="M"></typeparam>
    public interface IOriginator<T, M>
        where T : ISnapshoot
        where M : IMemento<T>, new()
    {
        IMemento<T> Memento { get; }
    }

    public abstract class OriginatorBase<T, M>
        where T : ISnapshoot, new()
        where M : IMemento<T>, new()
    {
        /// <summary>
        /// 发起人对象的状态
        /// </summary>
        protected T state;

        public OriginatorBase()
        {
            state = new T();
        }

        /// <summary>
        /// 把状态保存到备忘录，或者从备忘录恢复之前的状态
        /// </summary>
        public virtual IMemento<T> Memento
        {
            get
            {
                M m = new M();
                m.SetSnapshoot(this.state);
                return m;
            }
            set
            {
                if (value == null) throw new ArgumentNullException();
                this.state = value.GetSnapshoot();
            }
        }
    }   
}

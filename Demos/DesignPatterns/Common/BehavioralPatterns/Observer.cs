using System.Collections.Generic;
/// <summary>
/// 观察者模式是对象的行为模式，又叫发布-订阅(Publish/Subscribe)模式、模型-视图(Model/View)模式、
/// 源-监听器(Source/Listener)模式或从属者(Dependents)模式。观察者模式定义了一种一对多的依赖关系，
/// 让多个观察者对象同时监听某一个主题对象。这个主题对象在状态上发生变化时，会通知所有观察者对象，使它们能够自动更新自己。
/// </summary>
namespace Common.BehavioralPatterns
{
    /// <summary>
    /// 观察者类型接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObserver<T>
    {
        /// <summary>
        /// 拉模型
        /// </summary>
        /// <param name="subject"></param>
        void Update(ISubject<T> subject);

        ///// <summary>
        ///// 推模型
        ///// </summary>
        //void Update(T state);
    }

    /// <summary>
    /// 抽象的主题接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISubject<T>
    {
        void AttachObserver(IObserver<T> observer);

        void DetachObserver(IObserver<T> observer);

        T State { get; }
    }

    /// <summary>
    /// 目标对象抽象类型
    /// </summary>
    public class SubjectBase<T> : ISubject<T>
    {
        /// <summary>
        /// 登记所有需要通知的观察者
        /// </summary>
        private IList<IObserver<T>> observers = new List<IObserver<T>>();

        public T State { get; private set; }       

        /// <summary>
        /// 更新各观察者
        /// </summary>
        public virtual void Notify()
        {
            foreach (IObserver<T> observer in observers)
            {
                observer.Update(this);
            }
        }

        /// <summary>
        /// 供客户程序对目标对象进行操作的方法
        /// </summary>
        /// <param name="state"></param>
        public virtual void Update(T state)
        {
            this.State = state;
            Notify();   // 触发对外通知
        }

        public void AttachObserver(IObserver<T> observer)
        {
            this.observers.Add(observer);
        }

        public void DetachObserver(IObserver<T> observer)
        {
            this.observers.Remove(observer);
        }

        /// <summary>
        /// Attach
        /// </summary>
        public static SubjectBase<T> operator +(SubjectBase<T> subject, IObserver<T> observer)
        {
            subject.observers.Add(observer);
            return subject;
        }

        /// <summary>
        /// Detach
        /// </summary>
        public static SubjectBase<T> operator -(SubjectBase<T> subject, IObserver<T> observer)
        {
            subject.observers.Remove(observer);
            return subject;
        }
    }

    /// <summary>
    /// 具体观察者类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Observer<T> : IObserver<T>
    {
        public T State { get; set; }

        public void Update(ISubject<T> subject)
        {
            this.State = subject.State;
        }
    }
}

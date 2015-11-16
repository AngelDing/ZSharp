
using System;

namespace FlyweightPattern.ObjectPool
{
    /// <summary>
    /// 基本的抽象可池化类型
    /// <remarks>
    ///     由于其实例可以被重用，因此设计上不支持上下文状态信息
    /// </remarks>
    /// </summary>
    public abstract class PoolableBase : IPoolable
    {
        #region Constructor
        /// <summary>
        /// 构造池化对象的基本准备过程
        /// </summary>
        public PoolableBase()
        {
            this.Guid = System.Guid.NewGuid().ToString();
            this.Type = GetType();
            this.CreateTime = DateTime.Now;
            this.AccessedTime = DateTime.Now;
            this.State = ConstructedState.Instance;
        }
        #endregion

        #region Public Property
        /// <summary>
        /// 当前实例在内存的唯一标识
        /// </summary>
        public string Guid { get; private set; }

        /// <summary>
        /// 实例类型
        /// </summary>
        public virtual Type Type { get; private set; }

        /// <summary>
        /// 当前实例创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; private set; }

        /// <summary>
        /// 最近一次被访问时间
        /// </summary>
        public virtual DateTime AccessedTime { get; private set; }

        /// <summary>
        /// 当前实例的运行状态
        /// </summary>
        public virtual IState State { get; private set; }

        #endregion

        #region State Management

        /// <summary>
        /// 当前实例是否可以执行相应的功能
        /// </summary>
        public bool Executable
        {
            get { return State.Executable; }
        }

        /// <summary>
        /// 池化实例是否空闲着
        /// </summary>
        public bool Unoccupied
        {
            get { return State.Unoccupied; }
        }

        /// <summary>
        /// 修改当前的池化实例的状态
        /// </summary>
        /// <param name="newState"></param>
        public void ChangeState(IState newState)
        {
            this.State = newState;
        }

        /// <summary>
        /// 激活使用，此时排除其他对象对该实例的使用
        /// </summary>
        public virtual void Activate()
        {
            State.Activate(this);
        }

        /// <summary>
        /// 没有被客户程序占用，此时其他对象可以继续使用该实例
        /// </summary>
        public virtual void Deactivate()
        {
            State.Deactivate(this);
        }

        /// <summary>
        /// 释放, 此时其他对象不可以继续使用该实例
        /// </summary>
        public virtual void Dispose()
        {
            State.Dispose(this);
        }
        #endregion

        #region Helper method
        /// <summary>
        /// 更新最近被访问时间
        /// </summary>
        protected void PreProcess()
        {
            if (!Executable)
            {
                throw new NotSupportedException();
            }
            this.AccessedTime = DateTime.Now;
        }
        #endregion
    }
}

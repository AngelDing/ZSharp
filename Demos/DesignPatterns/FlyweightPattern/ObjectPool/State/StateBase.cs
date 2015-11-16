
namespace FlyweightPattern.ObjectPool
{
    /// <summary>
    /// 基础的状态类型
    /// </summary>
    public abstract class StateBase : IState
    {
        /// <summary>
        /// 当前状态下，池化实例是否可以执行
        /// </summary>
        public abstract bool Executable { get; }

        /// <summary>
        /// 当前状态下，池化实例是否空闲着
        /// </summary>
        public abstract bool Unoccupied { get; }

        /// <summary>
        /// 激活使用，此时排除其他对象对该实例的使用
        /// </summary>
        /// <param name="item"></param>
        public abstract void Activate(IPoolable item);

        /// <summary>
        /// 没有被客户程序占用，此时其他对象可以继续使用该实例
        /// </summary>
        /// <param name="item"></param>
        public abstract void Deactivate(IPoolable item);

        /// <summary>
        /// 释放, 此时其他对象不可以继续使用该实例
        /// </summary>
        /// <param name="item"></param>
        public virtual void Dispose(IPoolable item)
        {
            item.ChangeState(DestoryState.Instance);
        }
    }
}
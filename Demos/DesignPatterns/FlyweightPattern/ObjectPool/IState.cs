
namespace FlyweightPattern.ObjectPool
{
    /// <summary>
    /// 抽象状态对象
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// 当前状态下，池化实例是否可以执行
        /// </summary>
        bool Executable { get;}

        /// <summary>
        /// 当前状态下，池化实例是否空闲着
        /// </summary>
        bool Unoccupied { get;}

        /// <summary>
        /// 激活使用，此时排除其他对象对该实例的使用
        /// </summary>
        void Activate(IPoolable item);

        /// <summary>
        /// 没有被客户程序占用，此时其他对象可以继续使用该实例
        /// </summary>
        void Deactivate(IPoolable item);

        /// <summary>
        /// 释放, 此时其他对象不可以继续使用该实例
        /// </summary>
        void Dispose(IPoolable item);
    }
}

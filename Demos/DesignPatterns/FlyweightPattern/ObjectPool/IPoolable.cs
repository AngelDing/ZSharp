using System;

namespace FlyweightPattern.ObjectPool
{
    /// <summary>
    /// 基本可池化对象描述
    /// </summary>
    public interface IPoolable : IDisposable
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        string Guid { get;}             

        /// <summary>
        /// 实体类型
        /// </summary>
        Type Type { get;}
    
        /// <summary>
        /// 初次创建时间
        /// </summary>
        DateTime CreateTime { get;}     

        /// <summary>
        /// 最近访问时间
        /// </summary>
        DateTime AccessedTime { get;}

        /// <summary>
        /// 当前实例是否可以执行相应的功能
        /// </summary>
        bool Executable { get;}

        /// <summary>
        /// 池化实例是否空闲着
        /// </summary>
        bool Unoccupied { get;}

        /// <summary>
        /// 激活使用，此时排除其他对象对该实例的使用
        /// </summary>
        void Activate();

        /// <summary>
        /// 释放，此时其他对象可以继续使用该实例
        /// </summary>
        void Deactivate();

        /// <summary>
        /// 修改当前的池化实例的状态
        /// </summary>
        /// <param name="newState"></param>
        void ChangeState(IState newState);
    }
}

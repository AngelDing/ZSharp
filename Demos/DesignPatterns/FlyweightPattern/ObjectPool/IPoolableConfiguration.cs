
namespace FlyweightPattern.ObjectPool
{
    /// <summary>
    /// 运行时池化控制相关配置信息
    /// </summary>
    public interface IPoolableConfiguration
    {
        /// <summary>
        /// 最大的池化数量
        /// </summary>
        int Max { get;}

        /// <summary>
        /// 距最近一次“激活/释放”的超时时间
        /// </summary>
        int Timeout { get;}
    }
}

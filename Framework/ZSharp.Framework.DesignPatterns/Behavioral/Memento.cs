
namespace ZSharp.Framework.DesignPatterns
{
    /// <summary>
    /// 抽象备忘录对象接口
    /// </summary>
    public interface IMemento<T> 
    {
        T State { get; set; }
    }
}

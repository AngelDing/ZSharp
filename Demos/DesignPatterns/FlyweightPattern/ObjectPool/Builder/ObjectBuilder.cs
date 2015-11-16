
namespace FlyweightPattern.ObjectPool
{
    /// <summary>
    /// 池化对象构造器
    /// <remarks>
    /// 实际应用中由于对象的创建工作可能非常复杂，因此按照分工把、所有创建工作独立抽象出来。
    /// </remarks>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectBuilder<T> where T : class, IPoolable, new()
    {
        /// <summary>
        /// 创建指定类型实例
        /// </summary>
        /// <returns></returns>
        public T BuildUp()
        {
            return new T();
        }
    }
}
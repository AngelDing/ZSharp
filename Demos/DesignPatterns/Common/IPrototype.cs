using System;

namespace Common
{
    /// <summary>
    /// 原型模式，類似System.ICloneable接口
    /// </summary>
    public interface IPrototype
    {
        IPrototype Clone();
    }

    public class ConcretePrototype : IPrototype
    {
        /// <summary>
        /// 注意浅复制同深复制的区别，浅复制采用下面方法即可，深复制建议采用序列化方式
        /// </summary>
        /// <returns></returns>
        public IPrototype Clone()
        {
            return (IPrototype)this.MemberwiseClone();
        }
    }
}

using System;

namespace ZSharp.Framework.RabbitMq
{
    /// <summary>
    /// 指定消息是否需要持久化，默認採用Config中的值
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
    public class DeliveryModeAttribute : Attribute
    {
        public DeliveryModeAttribute(bool isPersistent)
        {
            IsPersistent = isPersistent;
        }

        public bool IsPersistent { get; private set; }
    }
}
using System;

namespace ZSharp.Framework.RabbitMq
{
    [Serializable]
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
using System;
using ZSharp.Framework.Configurations;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.RabbitMq
{
    public interface IMessageDeliveryModeStrategy
    {
        byte GetDeliveryMode(Type messageType);
    }

    public class MessageDeliveryModeStrategy : IMessageDeliveryModeStrategy
    {
        private readonly IRabbitMqConfiguration connectionConfiguration;

        public MessageDeliveryModeStrategy(IRabbitMqConfiguration connectionConfiguration)
        {
            this.connectionConfiguration = connectionConfiguration;
        }

        public byte GetDeliveryMode(Type messageType)
        {
            var deliveryModeAttribute = messageType.GetAttribute<DeliveryModeAttribute>();
            if (deliveryModeAttribute == null)
            {
                return GetDeliveryModeInternal(connectionConfiguration.PersistentMessages);
            }
            return GetDeliveryModeInternal(deliveryModeAttribute.IsPersistent);
        }

        private static byte GetDeliveryModeInternal(bool isPersistent)
        {
            return isPersistent ? MessageDeliveryMode.Persistent : MessageDeliveryMode.Transient;
        }
    }
}
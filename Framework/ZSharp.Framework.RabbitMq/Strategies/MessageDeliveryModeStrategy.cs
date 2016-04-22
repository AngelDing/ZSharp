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
        private readonly RabbitMqConfiguration connectionConfiguration;

        public MessageDeliveryModeStrategy(RabbitMqConfiguration connectionConfiguration)
        {
            //Preconditions.CheckNotNull(connectionConfiguration, "connectionConfiguration");
            this.connectionConfiguration = connectionConfiguration;
        }

        public byte GetDeliveryMode(Type messageType)
        {
            //Preconditions.CheckNotNull(messageType, "messageType");
            var deliveryModeAttribute = messageType.GetAttribute<DeliveryModeAttribute>();
            if (deliveryModeAttribute == null)
                return GetDeliveryModeInternal(connectionConfiguration.PersistentMessages);
            return GetDeliveryModeInternal(deliveryModeAttribute.IsPersistent);
        }

        private static byte GetDeliveryModeInternal(bool isPersistent)
        {
            return isPersistent ? MessageDeliveryMode.Persistent : MessageDeliveryMode.Transient;
        }
    }
}
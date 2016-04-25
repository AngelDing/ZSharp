using System;
using ZSharp.Framework.Serializations;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.RabbitMq
{
    public delegate string ExchangeNameConvention(Type messageType);
    public delegate string TopicNameConvention(Type messageType);
    public delegate string QueueNameConvention(Type messageType, string subscriberId);
    public delegate string ErrorQueueNameConvention();
    public delegate string ErrorExchangeNameConvention(MessageReceivedInfo info);
    public delegate string ConsumerTagConvention();

    public interface IConventions
    {
        ExchangeNameConvention ExchangeNamingConvention { get; set; }

        TopicNameConvention TopicNamingConvention { get; set; }

        QueueNameConvention QueueNamingConvention { get; set; }

        ErrorQueueNameConvention ErrorQueueNamingConvention { get; set; }

        ErrorExchangeNameConvention ErrorExchangeNamingConvention { get; set; }

        ConsumerTagConvention ConsumerTagConvention { get; set; }
    }

    public class Conventions : IConventions
    {
        public Conventions(ITypeNameSerializer typeNameSerializer)
        {
            // Establish default conventions.
            ExchangeNamingConvention = messageType =>
            {
                var attr = GetQueueAttribute(messageType);

                return string.IsNullOrEmpty(attr.ExchangeName)
                    ? typeNameSerializer.Serialize(messageType)
                    : attr.ExchangeName;
            };

            TopicNamingConvention = messageType => "";

            QueueNamingConvention =
                    (messageType, subscriptionId) =>
                    {
                        var attr = GetQueueAttribute(messageType);

                        if (string.IsNullOrEmpty(attr.QueueName))
                        {
                            var typeName = typeNameSerializer.Serialize(messageType);

                            return string.IsNullOrEmpty(subscriptionId)
                                ? typeName
                                : string.Format("{0}_{1}", typeName, subscriptionId);
                        }

                        return string.IsNullOrEmpty(subscriptionId)
                            ? attr.QueueName
                            : string.Format("{0}_{1}", attr.QueueName, subscriptionId);
                    };
            ErrorQueueNamingConvention = () => "ZRabbitMq_Default_Error_Queue";
            ErrorExchangeNamingConvention = info => "ErrorExchange_" + info.RoutingKey;
            ConsumerTagConvention = () => Guid.NewGuid().ToString();
        }

        private QueueAttribute GetQueueAttribute(Type messageType)
        {
            return messageType.GetAttribute<QueueAttribute>() ?? new QueueAttribute(string.Empty);
        }

        public ExchangeNameConvention ExchangeNamingConvention { get; set; }

        public TopicNameConvention TopicNamingConvention { get; set; }

        public QueueNameConvention QueueNamingConvention { get; set; }

        public ErrorQueueNameConvention ErrorQueueNamingConvention { get; set; }

        public ErrorExchangeNameConvention ErrorExchangeNamingConvention { get; set; }

        public ConsumerTagConvention ConsumerTagConvention { get; set; }
    }
}

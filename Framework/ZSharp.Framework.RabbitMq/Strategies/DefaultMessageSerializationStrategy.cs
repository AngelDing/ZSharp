using ZSharp.Framework.Serializations;

namespace ZSharp.Framework.RabbitMq
{
    public class DefaultMessageSerializationStrategy : IMessageSerializationStrategy
    {
        private readonly ITypeNameSerializer typeNameSerializer;
        private readonly ISerializer serializer;
        private readonly ICorrelationIdGenerationStrategy correlationIdGenerator;

        public DefaultMessageSerializationStrategy(ITypeNameSerializer typeNameSerializer, ISerializer serializer, ICorrelationIdGenerationStrategy correlationIdGenerator)
        {
            this.typeNameSerializer = typeNameSerializer;
            this.serializer = serializer;
            this.correlationIdGenerator = correlationIdGenerator;
        }

        public SerializedMessage SerializeMessage(IMessage message)
        {
            var typeName = typeNameSerializer.Serialize(message.MessageType);
            var messageBody = serializer.Serialize<byte[]>(message.GetBody());
            var messageProperties = message.Properties;

            messageProperties.Type = typeName;
            if (string.IsNullOrEmpty(messageProperties.CorrelationId))
            {
                messageProperties.CorrelationId = correlationIdGenerator.GetCorrelationId();
            }
            return new SerializedMessage(messageProperties, messageBody);
        }

        public IMessage<T> DeserializeMessage<T>(MessageProperties properties, byte[] body) where T : class
        {
            var messageBody = serializer.Deserialize<T>(body);
            return new Message<T>(messageBody, properties);
        }

        public IMessage DeserializeMessage(MessageProperties properties, byte[] body)
        {
            var messageType = typeNameSerializer.Deserialize(properties.Type);
            var messageBody = serializer.Deserialize(body, messageType);
            return MessageFactory.CreateInstance(messageType, messageBody, properties);
        }
    }
}
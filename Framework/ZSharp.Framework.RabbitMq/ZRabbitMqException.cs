using System;
using System.Runtime.Serialization;

namespace ZSharp.Framework.RabbitMq
{
    [Serializable]
    public class ZRabbitMqException : Exception
    {
        public ZRabbitMqException() {}
        public ZRabbitMqException(string message) : base(message) {}
        public ZRabbitMqException(string format, params string[] args) : base(string.Format(format, args)) {}
        public ZRabbitMqException(string message, Exception inner) : base(message, inner) {}

        protected ZRabbitMqException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) {}
    }

    [Serializable]
    public class ZRabbitMqInvalidMessageTypeException : ZRabbitMqException
    {
        public ZRabbitMqInvalidMessageTypeException() {}
        public ZRabbitMqInvalidMessageTypeException(string message) : base(message) {}
        public ZRabbitMqInvalidMessageTypeException(string format, params string[] args) : base(format, args) {}
        public ZRabbitMqInvalidMessageTypeException(string message, Exception inner) : base(message, inner) {}
        protected ZRabbitMqInvalidMessageTypeException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }

    [Serializable]
    public class ZRabbitMqResponderException : ZRabbitMqException
    {
        public ZRabbitMqResponderException() { }
        public ZRabbitMqResponderException(string message) : base(message) { }
        public ZRabbitMqResponderException(string format, params string[] args) : base(format, args) { }
        public ZRabbitMqResponderException(string message, Exception inner) : base(message, inner) { }
        protected ZRabbitMqResponderException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
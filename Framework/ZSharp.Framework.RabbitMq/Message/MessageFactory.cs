using System;
using System.Collections.Concurrent;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.RabbitMq
{
    /// <summary>
    /// Creates a generic <see cref="IMessage{T}"/> and returns it casted as <see cref="IMessage"/>
    /// so it can be used in scenarios where we only have a runtime <see cref="Type"/> available. 
    /// </summary>
    public static class MessageFactory
    {
        private static readonly ConcurrentDictionary<Type, Type> genericMessageTypesMap = new ConcurrentDictionary<Type, Type>();

        public static IMessage CreateInstance(Type messageType, object body)
        {
            GuardHelper.ArgumentNotNull(() => messageType);
            GuardHelper.ArgumentNotNull(() => body);

            var genericType = genericMessageTypesMap.GetOrAdd(messageType, t => typeof(Message<>).MakeGenericType(messageType));
            var message = ReflectionHelper.CreateInstance(genericType, body);
            return (IMessage)message;
        }

        public static IMessage CreateInstance(Type messageType, object body, MessageProperties properties)
        {
            GuardHelper.ArgumentNotNull(() => messageType);
            GuardHelper.ArgumentNotNull(() => body);
            GuardHelper.ArgumentNotNull(() => properties);

            var genericType = genericMessageTypesMap.GetOrAdd(messageType, t => typeof(Message<>).MakeGenericType(messageType));
            var message = ReflectionHelper.CreateInstance(genericType, body, properties);
            return (IMessage)message;
        }
    }
}

using System;
using System.Collections.Concurrent;
using ZSharp.Framework.Exceptions;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.Serializations
{
    public interface ITypeNameSerializer
    {
        string Serialize(Type type);

        Type Deserialize(string typeName);
    }

    /// <summary>
    /// 應註冊為單例
    /// </summary>
    public class TypeNameSerializer : ITypeNameSerializer
    {
        private readonly ConcurrentDictionary<string, Type> deserializedTypes = new ConcurrentDictionary<string, Type>();
        private readonly ConcurrentDictionary<Type, string> serializedTypes = new ConcurrentDictionary<Type, string>();

        public Type Deserialize(string typeName)
        {
            GuardHelper.ArgumentNotEmpty(() => typeName);

            return deserializedTypes.GetOrAdd(typeName, t =>
            {
                var nameParts = t.Split(':');
                if (nameParts.Length != 2)
                {
                    throw new FrameworkException("type name {0}, is not a valid type name. Expected Type:Assembly", t);
                }
                var type = Type.GetType(nameParts[0] + ", " + nameParts[1]);
                if (type == null)
                {
                    throw new FrameworkException("Cannot find type {0}", t);
                }
                return type;
            });
        }

        public string Serialize(Type type)
        {
            GuardHelper.ArgumentNotNull(() => type);

            return serializedTypes.GetOrAdd(type, t =>
            {
                var typeName = t.FullName + ":" + t.Assembly.GetName().Name;
                if (typeName.Length > 255)
                {
                    throw new FrameworkException("The serialized name of type '{0}' exceeds the AMQP " +
                                                "maximum short string length of 255 characters.", t.Name);
                }
                return typeName;
            });
        }
    }
}

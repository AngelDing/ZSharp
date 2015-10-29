using System;

namespace ZSharp.Framework.Serializations
{
    public class NullSerializer : BaseSerializer, ISerializer
    {       
        internal override SerializationFormat GetSerializationFormat()
        {
            return SerializationFormat.Null;
        }

        internal override object DoSerialize<T>(object item)
        {
            return null;
        }

        internal override T DoDeserialize<T>(object serializedObject)
        {
            return default(T);
        }

        internal override object DoDeserialize(object serializedObject, Type type)
        {
            return null;
        }
    }
}

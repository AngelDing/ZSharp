using MsgPack.Serialization;
using System;
using System.IO;
using System.Text;

namespace ZSharp.Framework.Serializations
{
    /// <summary>
    /// 不支持循環引用
    /// </summary>
    public class MsgPackSerializer : BaseSerializer, ISerializer
    {
        internal override object DoDeserialize(object serializedObject, Type type)
        {
            byte[] data = GetEncodingBytes(serializedObject);
            using (MemoryStream stream = new MemoryStream(data))
            {
                // Creates serializer.
                var serializer = SerializationContext.Default.GetSerializer(type);
                // Pack obj to stream.
                return serializer.Unpack(stream);
            }
        }

        internal override T DoDeserialize<T>(object serializedObject)
        {
            byte[] data = GetEncodingBytes(serializedObject);
            using (MemoryStream stream = new MemoryStream(data))
            {
                // Creates serializer.
                var serializer = SerializationContext.Default.GetSerializer<T>();
                // Pack obj to stream.
                return serializer.Unpack(stream);
            }
        }

        internal override object DoSerialize<T>(object item)
        {
            byte[] result;
            var serializer = SerializationContext.Default.GetSerializer(item.GetType());
            using (var stream = new MemoryStream())
            {
                serializer.Pack(stream, item);
                result = stream.ToArray();
            }

            if (typeof(T) == typeof(string))
            {
                return GetEncodingString(result);
            }
            return result;
        }

        internal override SerializationFormat GetSerializationFormat()
        {
            return SerializationFormat.MsgPack;
        }
    }
}

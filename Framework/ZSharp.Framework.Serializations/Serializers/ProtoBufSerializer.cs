using System;
using System.IO;
using System.Text;

namespace ZSharp.Framework.Serializations
{
    /// <summary>
    /// 1.不支持循環引用;
    /// 2.不支持DateTimeOffset數據類型;
    /// 3.不支持泛型；
    /// </summary>
    public class ProtoBufSerializer : BaseSerializer, ISerializer
    {
        internal override T DoDeserialize<T>(object serializedObject)
        {
            var buffer = GetEncodingBytes(serializedObject); 
            T result = default(T);
            using (var stream = new MemoryStream(buffer))
            {
                result = ProtoBuf.Serializer.Deserialize<T>(stream);
            }
            return result;
        }

        internal override object DoSerialize<T>(object item)
        {
            byte[] result;
            using (var stream = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(stream, item);
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
            return SerializationFormat.ProtoBuf;
        }

        internal override object DoDeserialize(object serializedObject, Type type)
        {
            throw new NotImplementedException("ProtoBufSerializer暫不支持按傳入的type進行反序列化！");
        }
    }
}

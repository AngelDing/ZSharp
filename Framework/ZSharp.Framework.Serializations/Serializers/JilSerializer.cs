using Jil;
using System;
using System.Text;

namespace ZSharp.Framework.Serializations
{
    /// <summary>
    /// 1.不支持循環引用；
    /// 2.集合不支持ICollection，但支持IList
    /// 3.注意DateTime類型，反序列化后日期要晚8小時,建議使用DateTimeOffset類型
    /// </summary>
    public class JilSerializer : BaseSerializer,ISerializer
	{
        private readonly Options jilOptions;
        public JilSerializer()
        {
            this.IsDefaultString = true;
            jilOptions = new Options(dateFormat: DateTimeFormat.ISO8601, includeInherited: true);
        }

        internal override SerializationFormat GetSerializationFormat()
        {
            return SerializationFormat.Jil;
        }

        internal override object DoSerialize<T>(object item)
        {
            var result = JSON.Serialize(item, jilOptions);
            if (typeof(T) == typeof(byte[]))
            {
                return this.GetEncodingBytes(result);
            }
            return result;
        }

        internal override T DoDeserialize<T>(object serializedObject)
        {
            var result = GetEncodingString(serializedObject);
            return JSON.Deserialize<T>(result, jilOptions);
        }

        internal override object DoDeserialize(object serializedObject, Type type)
        {
            var result = GetEncodingString(serializedObject);
            return JSON.Deserialize(result, type, jilOptions);
        }
    }
}
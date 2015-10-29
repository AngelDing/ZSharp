using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ZSharp.Framework.Serializations
{
    /// <summary>
    /// 支持循環引用
    /// </summary>
    public class BinarySerializer : BaseSerializer, ISerializer
    {
        internal override SerializationFormat GetSerializationFormat()
        {
            return SerializationFormat.Binary;
        }      

        internal override object DoSerialize<T>(object value)
        {          
            byte[] result;
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, value);
                stream.Flush();
                result = stream.ToArray();
            }

            if (typeof(T) == typeof(string))
            {
                return this.GetEncodingString(result);
            }

            return result;
        }

        internal override T DoDeserialize<T>(object serializedObject)
        {
            T result = default(T);
            var obj = Deserialize(serializedObject, typeof(T));
            if (null != obj)
            {
                result = (T)obj;
            }
            return result;
        }

        internal override object DoDeserialize(object serializedObject, Type type)
        {
            byte[] buffer = this.GetEncodingBytes(serializedObject);
            
            object result;
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream(buffer))
            {
                result = formatter.Deserialize(stream);
            }
            return result;
        }        
    }
}

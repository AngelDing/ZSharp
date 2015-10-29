using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ZSharp.Framework.Serializations
{
    /// <summary>
    /// 1.不支持循環引用；
    /// 2.不支持IList,ICollection等接口，但支持List；
    /// 3.不支持TimeSpan數據類型；
    /// </summary>
    public class XmlSerializer : BaseSerializer, ISerializer
    {      
        internal override SerializationFormat GetSerializationFormat()
        {
            return SerializationFormat.Xml;
        }

        internal override object DoSerialize<T>(object item)
        {
            byte[] result;
            var xmlSer = new System.Xml.Serialization.XmlSerializer(item.GetType());
            using (var ms = new MemoryStream())
            {
                xmlSer.Serialize(ms, item);
                result = ms.ToArray();
            }

            if (typeof(T) == typeof(string))
            {
                return GetEncodingString(result);
            }

            return result;
        }

        internal override T DoDeserialize<T>(object serializedObject)
        {
            T result = default(T);
            var obj = DoDeserialize(serializedObject, typeof(T));
            if (null != obj)
            {
                result = (T)obj;
            }
            return result;
        }

        internal override object DoDeserialize(object serializedObject, Type type)
        {
            object result = null;
            var bytes = GetEncodingBytes(serializedObject);
            var xmlSer = new System.Xml.Serialization.XmlSerializer(type);
            using (var stream = new MemoryStream(bytes))
            {               
                result = xmlSer.Deserialize(stream);
            }
            return result;
        }
    }
}

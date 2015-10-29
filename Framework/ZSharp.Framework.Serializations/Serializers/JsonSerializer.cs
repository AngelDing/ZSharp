using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Text;

namespace ZSharp.Framework.Serializations
{
    /// <summary>
    /// 支持循環引用
    /// </summary>
    public class JsonSerializer : BaseSerializer, ISerializer
    {
        public JsonSerializer()
        {
            this.IsDefaultString = true;
        }

        internal override object DoDeserialize(object serializedObject, Type type)
        {
            var result = GetEncodingString(serializedObject);
            return JsonConvert.DeserializeObject(result, type, GetSettings());
        }

        internal override T DoDeserialize<T>(object serializedObject)
        {
            var result = GetEncodingString(serializedObject);
            return JsonConvert.DeserializeObject<T>(result, GetSettings());
        }

        internal override object DoSerialize<T>(object item)
        {
            var result = JsonConvert.SerializeObject(item, GetSettings());
            if (typeof(T) == typeof(byte[]))
            {
                return GetEncodingBytes(result);
            }
            return result;
        }

        internal override SerializationFormat GetSerializationFormat()
        {
            return SerializationFormat.Json;
        }

        private static JsonSerializerSettings GetSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters = new JsonConverter[] { new StringEnumConverter() };
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            return settings;
        }
    }
}

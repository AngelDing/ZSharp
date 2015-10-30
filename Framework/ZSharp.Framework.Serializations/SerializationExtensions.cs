

namespace ZSharp.Framework.Serializations
{
    public static class SerializationExtensions
    {
        public static T Deserialize<T>(this ISerializer serializer, string serializedObject)
        {
            return serializer.Deserialize<T>(serializedObject);
        }

        public static T Deserialize<T>(this ISerializer serializer, byte[] serializedObject)
        {
            return serializer.Deserialize<T>(serializedObject);
        }

        public static string ToJson(this object input)
        {
            return SerializationHelper.Jil.Serialize<string>(input);
        }

        public static T FromJson<T>(this string input)
        {
            return SerializationHelper.Jil.Deserialize<T>(input);
        }
    }
}

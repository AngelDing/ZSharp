

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
    }
}

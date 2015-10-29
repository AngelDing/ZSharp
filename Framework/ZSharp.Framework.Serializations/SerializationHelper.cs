using ZSharp.Framework.Dependency;

namespace ZSharp.Framework.Serializations
{
    /// <summary>
    /// Wrapper for accessing ISerializer implementations
    /// </summary>
    public static class SerializationHelper
    {
        public static ISerializer Json     
        {
            get 
            { 
                return SimpleLocator<SerializerLocator>.Current.Resolve<JsonSerializer>();
            }
        }

        public static ISerializer Xml
        {
            get
            {
                return SimpleLocator<SerializerLocator>.Current.Resolve<XmlSerializer>();
            }
        }

        public static ISerializer Binary
        {
            get
            {
                return SimpleLocator<SerializerLocator>.Current.Resolve<BinarySerializer>();
            }
        }

        public static ISerializer Jil   
        {
            get
            {
                return SimpleLocator<SerializerLocator>.Current.Resolve<JilSerializer>();
            }
        }

        public static ISerializer MsgPack
        {
            get
            {
                return SimpleLocator<SerializerLocator>.Current.Resolve<MsgPackSerializer>();
            }
        }

        public static ISerializer ProtoBuf
        {
            get
            {
                return SimpleLocator<SerializerLocator>.Current.Resolve<ProtoBufSerializer>();
            }
        }
    }
}

using ZSharp.Framework.Dependency;
using System.Configuration;
using ZSharp.Framework.Extensions;
using ZSharp.Framework.Configurations;

namespace ZSharp.Framework.Serializations
{
    public class SerializerLocator : SimpleLocator
    {
        public override void RegisterDefaults(IContainer container)
        {
            container.Register<JsonSerializer>(() => new JsonSerializer());
            container.Register<XmlSerializer>(() => new XmlSerializer());
            container.Register<BinarySerializer>(() => new BinarySerializer());
            
            //需要在appsetting中定義採用的額外的序列化方式:
            var formatType = CommonConfig.SerializationFormatType;
            if (formatType.IsNullOrEmpty())
            {
                return;
            }

            formatType = formatType.ToUpper();
            var allType = "ALL";

            if (formatType == SerializationFormat.Jil.GetDescription().ToUpper() || formatType == allType)
            {
                container.Register<JilSerializer>(() => new JilSerializer());
            }

            if (formatType == SerializationFormat.MsgPack.GetDescription().ToUpper() || formatType == allType)
            {
                container.Register<MsgPackSerializer>(() => new MsgPackSerializer());
            }

            if (formatType == SerializationFormat.ProtoBuf.GetDescription().ToUpper() || formatType == allType)
            {
                container.Register<ProtoBufSerializer>(() => new ProtoBufSerializer());
            }
        }
    }
}

using ZSharp.Framework.Configurations;
using ZSharp.Framework.Dependency;
using ZSharp.Framework.Serializations;
using ZSharp.Framework.Extensions;

namespace ZSharp.Framework.Caching
{
    public class CacheLocator : SimpleLocator
    {  
        public override void RegisterDefaults(IContainer container)
        {
            container.Register<CacheManager<AspNetCache>>(
                () => new CacheManager<AspNetCache>(t => { return new AspNetCache(); }));
            container.Register<CacheManager<StaticCache>>(
                () => new CacheManager<StaticCache>(t => { return new StaticCache(); }));          
            container.Register<CacheManager<LRUCache>>(
                () => new CacheManager<LRUCache>(t => { return new LRUCache(); }));
            container.Register<CacheManager<RedisCache>>(
                () => new CacheManager<RedisCache>(t => { return new RedisCache(GetSerializer()); }));
        }

        private ISerializer GetSerializer()
        {
            ISerializer serializer = SerializationHelper.Json;
            var formatType = CommonConfig.SerializationFormatType;

            if (!formatType.IsNullOrEmpty())
            {
                formatType = formatType.ToUpper();
                if (formatType == SerializationFormat.Jil.GetDescription().ToUpper())
                {
                    serializer = SerializationHelper.Jil;
                }
                else if (formatType == SerializationFormat.MsgPack.GetDescription().ToUpper())
                {
                    serializer = SerializationHelper.MsgPack;
                }
                else if (formatType == SerializationFormat.ProtoBuf.GetDescription().ToUpper())
                {
                    serializer = SerializationHelper.ProtoBuf;
                }
            }

            return serializer;
        }
    }
}

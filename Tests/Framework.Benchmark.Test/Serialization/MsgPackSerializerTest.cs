using ZSharp.Framework.Serializations;

namespace Framework.Benchmark.Test.Serialization
{
    public class MsgPackSerializerTest : CommonSerializerTest
    {
        public override ISerializer GetSerializer()
        {
            return SerializationHelper.MsgPack;
        }
    }
}

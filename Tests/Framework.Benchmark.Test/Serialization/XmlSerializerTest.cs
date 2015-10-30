using ZSharp.Framework.Serializations;

namespace Framework.Benchmark.Test.Serialization
{
    public class XmlSerializerTest : CommonSerializerTest
    {
        public override ISerializer GetSerializer()
        {
            return SerializationHelper.Xml;
        }
    }
}

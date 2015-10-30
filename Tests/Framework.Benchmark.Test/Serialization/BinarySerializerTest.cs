using ZSharp.Framework.Serializations;

namespace Framework.Benchmark.Test.Serialization
{
    public class BinarySerializerTest : CommonSerializerTest
    {      
        public override ISerializer GetSerializer()
        {
            return SerializationHelper.Binary;
        }
    }
}

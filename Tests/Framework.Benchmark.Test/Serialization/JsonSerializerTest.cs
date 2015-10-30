using ZSharp.Framework.Serializations;

namespace Framework.Benchmark.Test.Serialization
{
    public class JsonSerializerTest : CommonSerializerTest
    {
        public override ISerializer GetSerializer()
        {
            return SerializationHelper.Json;
        }
    }
}

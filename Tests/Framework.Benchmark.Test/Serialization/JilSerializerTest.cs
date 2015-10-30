using ZSharp.Framework.Serializations;

namespace Framework.Benchmark.Test.Serialization
{
    public class JilSerializerTest : CommonSerializerTest
    {
        public override ISerializer GetSerializer()
        {
            return SerializationHelper.Jil;
        }      
    }    
}

using Framework.Test.Core.Serialization;
using Microsoft.Xunit;

namespace Framework.Benchmark.Test.Serialization
{
    public abstract class CommonSerializerTest : BaseSerializerTest
    {
        [Benchmark(Iterations = 10000)]
        public virtual void serialize_benchmark_test()
        {
            var result = GetSerializedSimpleObject();
        }

        [Benchmark(Iterations = 10000)]
        public virtual void deserialize_benchmark_test()
        {
            var serializedObj = GetSerializedComplexObject();
            var result = Serializer.Deserialize<ComplexObject>(serializedObj);
        }
    }
}

using FluentAssertions;
using ZSharp.Framework.Serializations;
using Xunit;
using Xunit.Abstractions;

namespace Framework.Serialization.Test
{
    [TestCaseOrderer("Framework.Test.Core.PriorityOrderer", "Framework.Test.Core")]
    public class JsonSerializerTest : CommonSerializerTest
    {
        public JsonSerializerTest(ITestOutputHelper output)
            : base(output)
        {
        }

        public override ISerializer GetSerializer()
        {
            return SerializationHelper.Json;
        }

        [Fact]
        public override void loop_object_serialize_test()
        {
            var result = GetSerializedLoopObject();
            result.Should().NotBeNull();
        }
    }
}

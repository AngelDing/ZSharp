using Xunit;
using FluentAssertions;
using Xunit.Abstractions;
using ZSharp.Framework.Serializations;

namespace Framework.Serialization.Test
{
    [TestCaseOrderer("Framework.Test.Core.PriorityOrderer", "Framework.Test.Core")]
    public class BinarySerializerTest : CommonSerializerTest
    {
        public BinarySerializerTest(ITestOutputHelper output)
            : base(output)
        {
        }

        public override ISerializer GetSerializer()
        {
            return SerializationHelper.Binary;
        }

        [Fact]
        public override void loop_object_serialize_test()
        {
            var result = GetSerializedLoopObject();
            result.Should().NotBeNull();
        }
    }
}

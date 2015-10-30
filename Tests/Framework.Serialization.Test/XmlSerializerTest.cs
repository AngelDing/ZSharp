using FluentAssertions;
using ZSharp.Framework.Serializations;
using Framework.Test.Core.Serialization;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Framework.Serialization.Test
{
    [TestCaseOrderer("Framework.Test.Core.PriorityOrderer", "Framework.Test.Core")]
    public class XmlSerializerTest : CommonSerializerTest
    {
        public XmlSerializerTest(ITestOutputHelper output)
            : base(output)
        {
        }

        public override ISerializer GetSerializer()
        {
            return SerializationHelper.Xml;
        }

        [Fact]
        public override void loop_object_serialize_test()
        {
            Action action = () => GetSerializedLoopObject();
            action.ShouldThrow<Exception>();
        }

        [Fact]
        public override void complex_object_deserialize_test()
        {
            var serializedObj = GetSerializedComplexObject();
            var result = Serializer.Deserialize<ComplexObject>(serializedObj);
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.ListObjects.Count.Should().Be(3);
            result.OrderItem.Price.Should().Be(20);
            result.TimeSpan.Should().Be(new TimeSpan());
        }
    }
}

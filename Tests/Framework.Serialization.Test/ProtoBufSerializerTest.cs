using FluentAssertions;
using Framework.Test.Core.Entities;
using ZSharp.Framework.Serializations;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Framework.Serialization.Test
{
    [TestCaseOrderer("Framework.Test.Core.PriorityOrderer", "Framework.Test.Core")]
    public class ProtoBufSerializerTest : CommonSerializerTest
    {
        public ProtoBufSerializerTest(ITestOutputHelper output)
            : base(output)
        {
        }

        public override ISerializer GetSerializer()
        {
            return SerializationHelper.ProtoBuf;
        }

        [Fact]
        public override void loop_object_serialize_test()
        {
            Action action = () => GetSerializedLoopObject();
            action.ShouldThrow<Exception>();
        }

        [Fact]
        public override void generics_object_test()
        {
            Action action = () => GetSerializedGenericsObject();
            action.ShouldThrow<Exception>();
        }

        [Fact]
        public override void deserialize_by_type_test()
        {
            var testEvent = new TestEvent
            {
                SourceId = Guid.NewGuid(),
                Name = "Jacky",
                Price = 100,
                CreatedDate = DateTime.Now
            };

            var payload = this.Serializer.Serialize<string>(testEvent);
            
            Action action = () => this.Serializer.Deserialize(payload, typeof(TestEvent));
            action.ShouldThrow<NotImplementedException>("ProtoBufSerializer暫不支持按傳入的type進行反序列化！");

        }
    }
}

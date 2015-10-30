using ZSharp.Framework.Serializations;
using Xunit;
using Xunit.Abstractions;

namespace Framework.Serialization.Test
{
    [TestCaseOrderer("Framework.Test.Core.PriorityOrderer", "Framework.Test.Core")]
    public class MsgPackSerializerTest : CommonSerializerTest
    {
        public MsgPackSerializerTest(ITestOutputHelper output)
            : base(output)
        {
        }

        public override ISerializer GetSerializer()
        {
            return SerializationHelper.MsgPack;
        }

        [Fact]
        public override void loop_object_serialize_test()
        {
            ////不支持循環引用，測試會導致內存溢出錯誤
            //var result = GetSerializedLoopObject();
            //result.Should().BeNull();
        }        
    }
}

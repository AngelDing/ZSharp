using Framework.Test.Core.Serialization;
using FluentAssertions;
using ZSharp.Framework.Serializations;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Framework.Serialization.Test
{
    [TestCaseOrderer("Framework.Test.Core.PriorityOrderer", "Framework.Test.Core")]
    public class JilSerializerTest : CommonSerializerTest
    {
        public JilSerializerTest(ITestOutputHelper output)
            : base(output)
        {
        } 

        public override ISerializer GetSerializer()
        {
            return SerializationHelper.Jil;
        }

        [Fact]
        public override void loop_object_serialize_test()
        {
            ////不支持循環引用，測試會導致內存溢出錯誤
            //var result = GetSerializedLoopObject();
            //result.Should().BeNull();
        }

        [Fact]
        public override void generics_object_test()
        {
            var serializedObj = GetSerializedGenericsObject();
            var result = Serializer.Deserialize<GenericsObject<DateTime>>(serializedObj);

            result.Should().NotBeNull();
            var dateNow = result.Value.AddHours(8);
            dateNow.Year.Should().Be(DateTime.Now.Year);
            dateNow.Month.Should().Be(DateTime.Now.Month);
            dateNow.Day.Should().Be(DateTime.Now.Day);
            dateNow.Hour.Should().Be(DateTime.Now.Hour);
            dateNow.Minute.Should().Be(DateTime.Now.Minute);
        }

        [Fact]
        public void tojson_test()
        {
            var obj = this.CreateComplexObject();
            var result = obj.ToJson();
            result.Should().BeOfType(typeof(string));
            result.Contains("Name").Should().BeTrue();
            result.Should().NotBeEmpty();
        }

        [Fact]
        public void fromjson_test()
        {
            var obj = this.CreateComplexObject() as ComplexObject;
            var json = obj.ToJson();
            json.Should().BeOfType(typeof(string));
            json.Contains("Name").Should().BeTrue();
            json.Should().NotBeEmpty();

            var obj2 = json.FromJson<ComplexObject>();

            obj.OrderId.Should().Equals(obj2.OrderId);

        }
    }    
}

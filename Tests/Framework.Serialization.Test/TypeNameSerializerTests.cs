using System;
using Xunit;
using FluentAssertions;
using ZSharp.Framework.Serializations;
using ZSharp.Framework;

namespace Framework.Serialization.Test
{
    public class TypeNameSerializerTests
    {
        const string expectedTypeName = "System.String:mscorlib";
        private const string expectedCustomTypeName = "Framework.Serialization.Test.SomeRandomClass:Framework.Serialization.Test";
        private ITypeNameSerializer typeNameSerializer;

        public TypeNameSerializerTests()
        {
            typeNameSerializer = new TypeNameSerializer();
        }

        [Fact]
        public void Should_serialize_a_type_name()
        {
            var typeName = typeNameSerializer.Serialize(typeof(string));
            typeName.Should().Be(expectedTypeName);
        }

        [Fact]
        public void Should_serialize_a_custom_type()
        {
            var typeName = typeNameSerializer.Serialize(typeof(SomeRandomClass));
            typeName.Should().Be(expectedCustomTypeName);
        }

        [Fact]
        public void Should_deserialize_a_type_name()
        {
            var type = typeNameSerializer.Deserialize(expectedTypeName);
            type.Should().Be(typeof(string));
        }

        [Fact]
        public void Should_deserialize_a_custom_type()
        {
            var type = typeNameSerializer.Deserialize(expectedCustomTypeName);
            type.Should().Be(typeof(SomeRandomClass));
        }

        [Fact]
        public void Should_throw_exception_when_type_name_is_not_recognised()
        {
            Action action = () => typeNameSerializer.Deserialize("XXXX.TypeNameSerializer.None:XXXX");
            action.ShouldThrow<FrameworkException>();
        }

        [Fact]
        public void Should_throw_exception_if_type_name_is_null()
        {
            Action action = () => typeNameSerializer.Deserialize(null);
            action.ShouldThrow<ArgumentException>();
        }
    }

    public class SomeRandomClass
    {        
    }
}

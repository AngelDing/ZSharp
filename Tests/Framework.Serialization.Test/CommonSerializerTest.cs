using FluentAssertions;
using Framework.Test.Core.Serialization;
using System;
using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;
using ZSharp.Framework.Extensions;
using Framework.Test.Core;
using Framework.Test.Core.Entities;

namespace Framework.Serialization.Test
{    
    public abstract class CommonSerializerTest : BaseSerializerTest
    {
        private int iterations = 10000;
        public ITestOutputHelper TestOutput { get; private set; }

        public CommonSerializerTest(ITestOutputHelper output)
        {
            this.TestOutput = output;
        }

        public abstract void loop_object_serialize_test();

        [Fact]
        public virtual void simple_object_serialize_test()
        {
            var result = GetSerializedSimpleObject();
            result.Should().NotBeNull();
        }

        [Fact]
        public virtual void complex_object_serialize_test()
        {
            var result = GetSerializedComplexObject();
            result.Should().NotBeNull();
        }

        [Fact]
        public virtual void complex_object_deserialize_test()
        {
            var serializedObj = GetSerializedComplexObject();
            var result = Serializer.Deserialize<ComplexObject>(serializedObj);
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.ListObjects.Count.Should().Be(3);
            result.OrderItem.Price.Should().Be(20);
            result.TimeSpan.Should().Be(new TimeSpan(1, 1, 1));
        }

        [Fact]
        public virtual void generics_object_test()
        {
            var serializedObj = GetSerializedGenericsObject();
            var result = Serializer.Deserialize<GenericsObject<DateTime>>(serializedObj);

            result.Should().NotBeNull();
            var dateNow = result.Value;
            dateNow.Year.Should().Be(DateTime.Now.Year);
            dateNow.Month.Should().Be(DateTime.Now.Month);
            dateNow.Day.Should().Be(DateTime.Now.Day);
            dateNow.Hour.Should().Be(DateTime.Now.Hour);
            dateNow.Minute.Should().Be(DateTime.Now.Minute);
        }         

        [Fact, TestPriority(0)]
        public virtual void serialize_benchmark_test()
        {
            for (var i = 0; i < 100; i++)
            {
                GetSerializedComplexObject();
            }

            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < iterations; i++)
            {
                GetSerializedComplexObject();
            }
            stopwatch.Stop();
            var milliseconds = (decimal)stopwatch.Elapsed.TotalMilliseconds;
            var format = this.Serializer.Format.GetDescription();

            TestOutput.WriteLine(
                "Serialization Format : {0}, Iterations : {1}, Serialize Totals: {2}ms.", 
                format, iterations, milliseconds);
        }

        [Fact, TestPriority(1)]
        public virtual void deserialize_benchmark_test()
        {
            var serializedObj = GetSerializedComplexObject();
            for (var i = 0; i < 100; i++)
            {                
                Serializer.Deserialize<ComplexObject>(serializedObj);
            }

            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < iterations; i++)
            {
                Serializer.Deserialize<ComplexObject>(serializedObj);
            }
            stopwatch.Stop();
            var milliseconds = (decimal)stopwatch.Elapsed.TotalMilliseconds;
            var format = this.Serializer.Format.GetDescription();

            TestOutput.WriteLine(
                "Serialization Format : {0}, Iterations : {1}, Deserialize Totals: {2}ms.",
                format, iterations, milliseconds);
        }

        [Fact]
        public virtual void deserialize_by_type_test()
        {
            var testEvent = new TestEvent
            {
                SourceId = Guid.NewGuid(),
                Name = "Jacky",
                Price = 100,
                CreatedDate = DateTime.Now
            };

            var payload = this.Serializer.Serialize<string>(testEvent);
            var obj = this.Serializer.Deserialize(payload, typeof(TestEvent));

            var eventObj = (IEvent)obj;
            eventObj.Should().NotBeNull();
        }       
    }
}

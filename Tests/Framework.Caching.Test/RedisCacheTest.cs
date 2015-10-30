using Xunit;
using System;
using FluentAssertions;
using Xunit.Abstractions;
using System.Diagnostics;
using ZSharp.Framework.Caching;

namespace Framework.Caching.Test
{
	public class RedisCacheTest : BaseCacheTest
	{
        public ITestOutputHelper output;
        public RedisCacheTest(ITestOutputHelper output)
            : base()
        {
            this.output = output;
        }

        public override ICacheManager GetCacheManager()
        {
            return CacheHelper.RedisCache;
        }

        [Fact]
        public void cache_redis_constructor_test()
        {
            Action action = () => new RedisCache();
            action.ShouldNotThrow();
        }

        [Fact]
        public void cache_redis_set_complex_test()
        {
            var testobject = new TestClass<DateTime>();
            testobject.Key = "Jacky";
            testobject.Value = DateTime.Now;
            CacheManager.Set("my Key", testobject, 
                new CachePolicy()
                {
                    Duration = TimeSpan.FromSeconds(20),
                    ExpirationType = CacheExpirationType.Absolute
                });
            var result = CacheManager.Get<TestClass<DateTime>>("my Key");

            result.Should().NotBeNull();
            result.Key.Should().Be(testobject.Key);
            //Jil默認將DataTime類型轉為Utc時間，使用時注意同LocalTime的區別
            var newValue = result.Value.ToString("yyyy/MM/dd HH:mm:ss");
            newValue.Should().Be(testobject.Value.ToString("yyyy/MM/dd HH:mm:ss"));
        }

        [Fact]
        public override void cache_remove_by_pattern_test()
        {
            var key1 = "Key:Jakcy:1";
            var key2 = "Key:JakcyX:2";
            var key3 = "Key:JakcyX:3";
            CacheManager.Set(key1, "my value");
            CacheManager.Set(key2, "my value");
            CacheManager.Set(key3, "my value");
           
            CacheManager.RemoveByPattern("*:Jakcy:*");
            CacheManager.Get<string>(key1).Should().BeNullOrEmpty();
            CacheManager.Get<string>(key2).Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void cache_redis_base_type_test()
        {
            var key = "BaseTypeKey";
            CacheManager.Set(key, "string");
            var result = CacheManager.Get<string>(key);
            result.Should().Be("string");

            CacheManager.Set(key, 100);
            var result2 = CacheManager.Get<int>(key);
            result2.Should().Be(100);
        }

        [Fact]
        public void cache_redis_benchmark_test()
        {
            var key = "BaseTypeKey";
            CacheManager.Set(key, "string");
            var result = CacheManager.Get<string>(key);

            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < 10000; i++)
            {
                CacheManager.Set(key, "string");
                CacheManager.Get<string>(key);
            }
            stopwatch.Stop();
            var milliseconds = (decimal)stopwatch.Elapsed.TotalMilliseconds;
            output.WriteLine("Totals: {0}ms.",milliseconds);          
        }
	}
}

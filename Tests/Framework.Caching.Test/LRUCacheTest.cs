using Xunit;
using System;
using FluentAssertions;
using Xunit.Abstractions;
using System.Diagnostics;
using ZSharp.Framework.Caching;
using System.Globalization;
using ZSharp.Framework;

namespace Framework.Caching.Test
{
    public class LRUCacheTestclass : BaseCacheTest
    {
        public ITestOutputHelper output;
        public LRUCacheTestclass(ITestOutputHelper output)
            : base()
        {
            this.output = output;
        }

        public override ICacheManager GetCacheManager()
        {
            return CacheHelper.LRUCache;
        }

        [Fact]
        public override void cache_absolute_test()
        {
            string key = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
            var cacheKey = new CacheKey(key);
            var absoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(5);
            var cachePolicy = CachePolicy.WithAbsoluteExpiration(absoluteExpiration);
            var value = "Jacky zhou";
            this.CacheManager.Get<string>(cacheKey, () => { return value; }, cachePolicy);
            var expectValue = this.CacheManager.Get<string>(cacheKey, () => { return ""; });
            expectValue.Should().Be(value);
            //LRUCache由於是定時清理，故存在一定的時間誤差，不是嚴格意義上的過期時間一到就失效
            System.Threading.Thread.Sleep(6001);

            expectValue = this.CacheManager.Get<string>(cacheKey, () => { return ""; });
            expectValue.Should().Be("");
        }

        [Fact]
        public override void cache_sliding_test()
        {
            string key = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
            var cacheKey = new CacheKey(key);
            var slidingExpiration = TimeSpan.FromSeconds(3);
            var cachePolicy = CachePolicy.WithSlidingExpiration(slidingExpiration);
            var value = "Jacky zhou";

            this.CacheManager.Get<string>(cacheKey, () => { return value; }, cachePolicy);

            System.Threading.Thread.Sleep(1000);
            var expectValue = this.CacheManager.Get<string>(cacheKey, () => { return ""; }, cachePolicy);
            expectValue.Should().Be(value);

            System.Threading.Thread.Sleep(1000);
            expectValue = this.CacheManager.Get<string>(cacheKey, () => { return ""; }, cachePolicy);
            expectValue.Should().Be(value);

            System.Threading.Thread.Sleep(1000);
            expectValue = this.CacheManager.Get<string>(cacheKey, () => { return ""; }, cachePolicy);
            expectValue.Should().Be(value);

            System.Threading.Thread.Sleep(2999);
            expectValue = this.CacheManager.Get<string>(cacheKey, () => { return ""; }, cachePolicy);
            expectValue.Should().Be(value);

            System.Threading.Thread.Sleep(4001);
            expectValue = this.CacheManager.Get<string>(cacheKey, () => { return ""; });
            expectValue.Should().Be("");
        }

        [Fact]
        public void cache_max_size_test()
        {
            var key1 = "key1";
            var key2 = "key2";
            var key3 = "key3";
            this.CacheManager.Get<string>(key1, () => { return key1; });
            this.CacheManager.Get<string>(key2, () => { return key2; });
            this.CacheManager.Get<string>(key3, () => { return key3; });

            var result1 = this.CacheManager.Get<string>(key1);
            result1.Should().BeNullOrEmpty();
            var result2 = this.CacheManager.Get<string>(key2);
            result2.Should().Be(key2);
            var result3 = this.CacheManager.Get<string>(key3);
            result3.Should().Be(key3);
        }
    }
}

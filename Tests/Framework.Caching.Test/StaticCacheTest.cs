using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Caching;
using FluentAssertions;
using Xunit;
using ZSharp.Framework.Caching;

namespace Framework.Caching.Test
{
    public class StaticCacheTest : BaseCacheTest
    {
        //public StaticCacheTest()
        //    : base(CacheHelper.MemoryCache)
        //{
        //}

        public override ICacheManager GetCacheManager()
        {
            return CacheHelper.MemoryCache;
        }

        [Fact]
        public void cache_memary_constructor_test()
        {
            Action action = () => new StaticCache();
            action.ShouldNotThrow();
        }

        [Fact]
        public void cache_memary_create_change_monitor_test()
        {
            string key = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
            string[] tags = new[] { "a", "b" };
            var cacheKey = new CacheKey(key, tags);
            cacheKey.Should().NotBeNull();

            var provider = new StaticCache();
            var monitor = provider.CreateChangeMonitor(cacheKey);
            monitor.Should().NotBeNull();
            monitor.CacheKeys.Should().HaveCount(2);

            var cacheTag = new CacheTag("a");
            string tagKey = provider.GetTagKey(cacheTag);
            tagKey.Should().NotBeNullOrEmpty();

            monitor.CacheKeys.Should().Contain(tagKey);
        }

        [Fact]
        public void cache_memary_create_policy_absolute_test()
        {
            string key = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
            string[] tags = new[] { "a", "b" };
            var cacheKey = new CacheKey(key, tags);
            cacheKey.Should().NotBeNull();

            var absoluteExpiration = DateTimeOffset.Now.AddMinutes(5);
            var cachePolicy = CachePolicy.WithAbsoluteExpiration(absoluteExpiration);
            cachePolicy.Should().NotBeNull();
            var provider = new StaticCache();
            var policy = provider.CreatePolicy(cacheKey, cachePolicy);
            policy.Should().NotBeNull();
            policy.AbsoluteExpiration.Should().Be(absoluteExpiration);
            policy.ChangeMonitors.Should().NotBeNull();
            policy.ChangeMonitors.Should().HaveCount(1);
            policy.ChangeMonitors.Should().ContainItemsAssignableTo<CacheEntryChangeMonitor>();
        }

        [Fact]
        public void cache_memary_create_policy_sliding_test()
        {
            string key = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
            string[] tags = new[] { "a", "b" };
            var cacheKey = new CacheKey(key, tags);
            cacheKey.Should().NotBeNull();

            var slidingExpiration = TimeSpan.FromMinutes(5);
            var cachePolicy = CachePolicy.WithSlidingExpiration(slidingExpiration);
            cachePolicy.Should().NotBeNull();
            var provider = new StaticCache();
            var policy = provider.CreatePolicy(cacheKey, cachePolicy);
            policy.Should().NotBeNull();
            policy.SlidingExpiration.Should().Be(slidingExpiration);
            policy.ChangeMonitors.Should().NotBeNull();
            policy.ChangeMonitors.Should().HaveCount(1);
            policy.ChangeMonitors.Should().ContainItemsAssignableTo<CacheEntryChangeMonitor>();
        }  
    }
}

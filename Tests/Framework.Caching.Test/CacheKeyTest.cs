using System;
using System.Globalization;
using FluentAssertions;
using Xunit;
using ZSharp.Framework.Caching;

namespace Framework.Caching.Test
{
    public class CacheKeyTest
    {
        [Fact]
        public void cache_key_constructor_null_key_test()
        {
            Action action = () => new CacheKey(null);
            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void cache_key_constructor_null_tags_test()
        {
            Action action = () => new CacheKey("test", null);
            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void cache_key_constructor_test()
        {
            string key = string.Empty;
            var target = new CacheKey(key);
            target.Should().NotBeNull();
            target.Key.Should().NotBeNull();
            target.Key.Should().Be(string.Empty);
        }

        [Fact]
        public void cache_key_test()
        {
            string key = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
            var target = new CacheKey(key);
            target.Should().NotBeNull();
            target.Key.Should().NotBeNull();
            target.Key.Should().Be(key);
        }

        [Fact]
        public void cache_key_tags_test()
        {
            string key = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
            string[] tags = new[] { "a", "b" };
            var target = new CacheKey(key, tags);

            target.Should().NotBeNull();
            target.Key.Should().NotBeNull();
            target.Key.Should().Be(key);

            target.Tags.Should().HaveCount(2);
        }
    }
}

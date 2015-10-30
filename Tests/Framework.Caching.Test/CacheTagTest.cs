using System.Globalization;
using FluentAssertions;
using Xunit;
using System;
using ZSharp.Framework.Caching;

namespace Framework.Caching.Test
{
    public class CacheTagTest
    {
        [Fact]
        public void cache_tag_Constructor_null_test()
        {
            Action action = () => new CacheTag(null);
            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void cache_tag_constructor_empty_test()
        {
            var cacheTag = new CacheTag(string.Empty);
            cacheTag.Should().NotBeNull();
            cacheTag.ToString().Should().BeEmpty();
        }


        [Fact]
        public void cache_tag_constructor_test()
        {
            string tag = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
            var cacheTag = new CacheTag(tag);
            cacheTag.Should().NotBeNull();
            cacheTag.ToString().Should().Be(tag);
        }


        [Fact]
        public void cache_tag_equals_test()
        {
            string tag = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
            var leftTag = new CacheTag(tag);
            leftTag.Should().NotBeNull();
            leftTag.ToString().Should().Be(tag);

            var rightTag = new CacheTag(tag);
            rightTag.Should().NotBeNull();
            rightTag.ToString().Should().Be(tag);

            leftTag.Equals(rightTag).Should().BeTrue();

            (leftTag == rightTag).Should().BeTrue();
        }


        [Fact]
        public void cache_tag_get_hash_code_test()
        {
            string tag = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
            int hashCode = tag.GetHashCode();

            var cacheTag = new CacheTag(tag);
            cacheTag.Should().NotBeNull();
            cacheTag.ToString().Should().Be(tag);
            cacheTag.GetHashCode().Should().Be(hashCode);
        }
    }
}

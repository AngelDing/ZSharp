using Xunit;
using FluentAssertions;
using ZSharp.Framework.Redis;
using ZSharp.Framework;

namespace Framework.Redis.Test
{
    public class RedisWrapperTest : DisposableObject
    {
        private readonly IRedisWrapper redisWrapper;
        private const string keyTemplate = "my:test:key:{0}";

        public RedisWrapperTest()
        {
            this.redisWrapper = RedisFactory.GetRedisWrapper();
        }

        [Fact]
        public void redis_set_and_get_test()
        {
            var key = string.Format(keyTemplate, 1);
            redisWrapper.Set(key, "123");

            var value = redisWrapper.Get(key);
            value.Should().NotBeNull();

            value.ToString().Should().Be("123");
        }

        [Fact]
        public void redis_getset_test()
        {
            var key = string.Format(keyTemplate, 1);
            redisWrapper.Set(key, "123");

            var value = redisWrapper.GetSet(key, "321");
            value.ToString().Should().Be("123");

            value = redisWrapper.Get(key);
            value.ToString().Should().Be("321");
        }

        [Fact]
        public void redis_increment_test()
        {
            var key = string.Format(keyTemplate, 2);
            var value = redisWrapper.Increment(key);
            value.Should().Be(1);

            value = redisWrapper.Increment(key);
            value.Should().Be(2);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.redisWrapper.ClearAll();
            }
        }
    }
}

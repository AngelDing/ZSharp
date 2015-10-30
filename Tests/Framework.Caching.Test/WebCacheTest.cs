using System;
using ZSharp.Framework.Caching;
using Xunit;
using FluentAssertions;

namespace Framework.Caching.Test
{
    public class WebCacheTest  : BaseCacheTest
	{
        //      public WebCacheTest()
        //          : base(CacheHelper.WebCache)
        //{
        //}

        public override ICacheManager GetCacheManager()
        {
            return CacheHelper.WebCache;
        }

        [Fact]
        public void cache_web_constructor_test()
        {
            Action action = () => new StaticCache();
            action.ShouldNotThrow();
        }        
    }
}

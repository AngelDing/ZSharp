using System;
using System.Net.Http;
using System.Web.Http;
using Moq;
using Xunit;
using ZSharp.Framework.WebApi.OutputCache;

namespace Framework.WebApi.OutputCache.Tests
{
    public class ConfigurationTests
    {
        private HttpServer _server;
        private string _url = "http://www.strathweb.com/api/sample/";
        private Mock<IApiOutputCache> _cache;

        [Fact]
        public void cache_singleton_in_pipeline()
        {
            _cache = new Mock<IApiOutputCache>();

            var conf = new HttpConfiguration();
            conf.CacheOutputConfiguration().RegisterCacheOutputProvider(() => _cache.Object);

            conf.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

            _server = new HttpServer(conf);

            var client = new HttpClient(_server);
            var result = client.GetAsync(_url + "Get_c100_s100").Result;

            _cache.Verify(s => s.Contains(It.Is<string>(x => x == "framework.webapi.outputcache.tests.samplecontroller-get_c100_s100:application/json; charset=utf-8")), Times.Exactly(2));

            var result2 = client.GetAsync(_url + "Get_c100_s100").Result;
            _cache.Verify(s => s.Contains(It.Is<string>(x => x == "framework.webapi.outputcache.tests.samplecontroller-get_c100_s100:application/json; charset=utf-8")), Times.Exactly(4));

            _server.Dispose();
        }

        [Fact]
        public void cache_singleton()
        {
            var cache = new DefaultMemoryCache();

            var conf = new HttpConfiguration();
            conf.CacheOutputConfiguration().RegisterCacheOutputProvider(() => cache);

            object cache1;
            conf.Properties.TryGetValue(typeof(IApiOutputCache), out cache1);

            object cache2;
            conf.Properties.TryGetValue(typeof(IApiOutputCache), out cache2);

            Assert.Same(((Func<IApiOutputCache>)cache1)(), ((Func<IApiOutputCache>)cache2)());
        }

        [Fact]
        public void cache_instance()
        {
            var conf = new HttpConfiguration();
            conf.CacheOutputConfiguration().RegisterCacheOutputProvider(() => new DefaultMemoryCache());

            object cache1;
            conf.Properties.TryGetValue(typeof(IApiOutputCache), out cache1);

            object cache2;
            conf.Properties.TryGetValue(typeof(IApiOutputCache), out cache2);

            Assert.NotSame(((Func<IApiOutputCache>)cache1)(), ((Func<IApiOutputCache>)cache2)());
        }
    }
}
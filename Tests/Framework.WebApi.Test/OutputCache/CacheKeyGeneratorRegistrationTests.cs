using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using Autofac;
using Autofac.Integration.WebApi;
using Moq;
using Xunit;
using System.Diagnostics;
using ZSharp.Framework.WebApi.OutputCache;
using Xunit.Abstractions;

namespace Framework.WebApi.OutputCache.Tests
{
    public class CacheKeyGeneratorRegistrationTests
    {
        private HttpServer _server;
        private string _url = "http://www.strathweb.com/api/";
        private Mock<IApiOutputCache> _cache;
        private Mock<ICacheKeyGenerator> _keyGenerator;
        private readonly ITestOutputHelper testOutput;

        public CacheKeyGeneratorRegistrationTests(ITestOutputHelper testOutput)
        {
            Thread.CurrentPrincipal = null;
            this.testOutput = testOutput;

            _cache = new Mock<IApiOutputCache>();
            _keyGenerator = new Mock<ICacheKeyGenerator>();

            var conf = new HttpConfiguration();
            
            var builder = new ContainerBuilder();
            builder.RegisterInstance(_cache.Object);

            conf.DependencyResolver = new AutofacWebApiDependencyResolver(builder.Build());
            conf.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

            _server = new HttpServer(conf);
        }

        [Fact]
        public void registered_default_is_used()
        {
            _server.Configuration.CacheOutputConfiguration().RegisterDefaultCacheKeyGeneratorProvider(() => _keyGenerator.Object);

            var client = new HttpClient(_server);
            var result = client.GetAsync(_url + "sample/Get_c100_s100").Result;

            _keyGenerator.VerifyAll();
        }

        [Fact]
        public void last_registered_default_is_used()
        {
            _server.Configuration.CacheOutputConfiguration().RegisterDefaultCacheKeyGeneratorProvider(() =>
            {
                testOutput.WriteLine("First registration should have been overwritten");
                return null; 
            });
            _server.Configuration.CacheOutputConfiguration().RegisterDefaultCacheKeyGeneratorProvider(() => _keyGenerator.Object);

            var client = new HttpClient(_server);
            var result = client.GetAsync(_url + "sample/Get_c100_s100").Result;

            _keyGenerator.VerifyAll();
        }

        [Fact]
        public void specific_registration_does_not_affect_default()
        {
            _server.Configuration.CacheOutputConfiguration().RegisterDefaultCacheKeyGeneratorProvider(() => _keyGenerator.Object);
            _server.Configuration.CacheOutputConfiguration().RegisterCacheKeyGeneratorProvider(() => new FailCacheKeyGenerator(testOutput));

            var client = new HttpClient(_server);
            var result = client.GetAsync(_url + "sample/Get_c100_s100").Result;

            _keyGenerator.VerifyAll();
        }

        [Fact]
        public void selected_generator_with_internal_registration_is_used()
        {
            _server.Configuration.CacheOutputConfiguration().RegisterCacheKeyGeneratorProvider(() => new InternalRegisteredCacheKeyGenerator("internal"));

            var client = new HttpClient(_server);
            var result = client.GetAsync(_url + "cachekey/get_internalregistered").Result;

            _cache.Verify(s => s.Add(It.Is<string>(x => x == "internal"), It.IsAny<byte[]>(), It.Is<DateTimeOffset>(x => x <= DateTime.Now.AddSeconds(100)), It.Is<string>(x => x == "framework.webapi.outputcache.tests.cachekeycontroller-get_internalregistered")), Times.Once());
        }

        [Fact]
        public void custom_unregistered_cache_key_generator_called()
        {
            var client = new HttpClient(_server);
            var result = client.GetAsync(_url + "cachekey/get_unregistered").Result;

            _cache.Verify(s => s.Contains(It.Is<string>(x => x == "unregistered")), Times.Once());
        }

        #region Helper classes
        private class FailCacheKeyGenerator : ICacheKeyGenerator
        {
            private readonly ITestOutputHelper testOutput;

            public FailCacheKeyGenerator(ITestOutputHelper testOutput)
            {
                this.testOutput = testOutput;
            }
            public string MakeCacheKey(HttpActionContext context, MediaTypeHeaderValue mediaType, bool excludeQueryString = false)
            {
                testOutput.WriteLine("This cache key generator should never be invoked");
                return "fail";
            }
        }

        public class InternalRegisteredCacheKeyGenerator : ICacheKeyGenerator
        {
            private readonly string _key;

            public InternalRegisteredCacheKeyGenerator(string key)
            {
                _key = key;
            }

            public string MakeCacheKey(HttpActionContext context, MediaTypeHeaderValue mediaType, bool excludeQueryString = false)
            {
                return _key;
            }
        }
        #endregion
    }
}
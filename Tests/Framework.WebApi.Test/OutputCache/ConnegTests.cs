using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Moq;
using Xunit;
using ZSharp.Framework.WebApi.OutputCache;
using ZSharp.Framework;

namespace Framework.WebApi.OutputCache.Tests
{
    public class ConnegTests : DisposableObject
    {
        private HttpServer _server;
        private string _url = "http://www.strathweb.com/api/sample/";
        private Mock<IApiOutputCache> _cache;

        public ConnegTests()
        {
            _cache = new Mock<IApiOutputCache>();

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
        public void subsequent_xml_request_is_not_cached()
        {
            var client = new HttpClient(_server);
            var result = client.GetAsync(_url + "Get_c100_s100").Result;

            _cache.Verify(s => s.Contains(It.Is<string>(x => x == "framework.webapi.outputcache.tests.samplecontroller-get_c100_s100:application/json; charset=utf-8")), Times.Exactly(2));
            _cache.Verify(s => s.Add(It.Is<string>(x => x == "framework.webapi.outputcache.tests.samplecontroller-get_c100_s100:application/json; charset=utf-8"), It.IsAny<object>(), It.Is<DateTimeOffset>(x => x < DateTime.Now.AddSeconds(100)), It.Is<string>(x => x == "framework.webapi.outputcache.tests.samplecontroller-get_c100_s100")), Times.Once());

            var req = new HttpRequestMessage(HttpMethod.Get, _url + "Get_c100_s100");
            req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));

            var result2 = client.SendAsync(req).Result;
            _cache.Verify(s => s.Contains(It.Is<string>(x => x == "framework.webapi.outputcache.tests.samplecontroller-get_c100_s100:text/xml; charset=utf-8")), Times.Exactly(2));
            _cache.Verify(s => s.Add(It.Is<string>(x => x == "framework.webapi.outputcache.tests.samplecontroller-get_c100_s100:text/xml; charset=utf-8"), It.IsAny<object>(), It.Is<DateTimeOffset>(x => x < DateTime.Now.AddSeconds(100)), It.Is<string>(x => x == "framework.webapi.outputcache.tests.samplecontroller-get_c100_s100")), Times.Once());

        }
            
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_server != null) _server.Dispose();
            }
        }
    }
}

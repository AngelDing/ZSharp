using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Xunit;
using ZSharp.Framework;
using ZSharp.Framework.WebApi.OutputCache;

namespace ZSharp.Framework.WebApi.OutputCache.Tests
{
    public class ClientSideTests : DisposableObject
    {
        private HttpServer _server;
        private string _url = "http://www.strathweb.com/api/sample/";

        public ClientSideTests()
        {
            var conf = new HttpConfiguration();
            conf.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

            _server = new HttpServer(conf);
        }

        [Fact]
        public void maxage_mustrevalidate_false_headers_correct()
        {
            var client = new HttpClient(_server);
            var result = client.GetAsync(_url + "Get_c100_s100").Result;

            Assert.Equal(TimeSpan.FromSeconds(100), result.Headers.CacheControl.MaxAge);
            Assert.False(result.Headers.CacheControl.MustRevalidate);
        }

        [Fact]
        public void no_cachecontrol_when_clienttimeout_is_zero()
        {
            var client = new HttpClient(_server);
            var result = client.GetAsync(_url + "Get_c0_s100").Result;

            Assert.Null(result.Headers.CacheControl);
        }

        [Fact]
        public void no_cachecontrol_when_request_not_succes()
        {
            var client = new HttpClient(_server);
            var result = client.GetAsync(_url + "Get_request_httpResponseException_noCache").Result;

            Assert.Null(result.Headers.CacheControl);
        }

        [Fact]
        public void no_cachecontrol_when_request_exception()
        {
            var client = new HttpClient(_server);
            var result = client.GetAsync(_url + "Get_request_exception_noCache").Result;

            Assert.Null(result.Headers.CacheControl);
        }
        [Fact]
        public void maxage_cachecontrol_when_no_content()
        {
            var client = new HttpClient(_server);
            var result = client.GetAsync(_url + "Get_request_noContent").Result;

            Assert.NotNull(result.Headers.CacheControl);
            Assert.Equal(TimeSpan.FromSeconds(50), result.Headers.CacheControl.MaxAge);
        }


        [Fact]
        public void maxage_mustrevalidate_headers_correct_with_clienttimeout_zero_with_must_revalidate()
        {
            var client = new HttpClient(_server);
            var result = client.GetAsync(_url + "Get_c0_s100_mustR").Result;

            Assert.True(result.Headers.CacheControl.MustRevalidate);
            Assert.Equal(TimeSpan.Zero, result.Headers.CacheControl.MaxAge);
        }


	    [Fact]
	    public void nocache_headers_correct()
	    {
			var client = new HttpClient(_server);
			var result = client.GetAsync(_url + "Get_nocache").Result;

			Assert.True(result.Headers.CacheControl.NoCache,
				"NoCache in result headers was expected to be true when CacheOutput.NoCache=true.");
		    Assert.True(result.Headers.Contains("Pragma"),
				"result headers does not contain expected Pragma.");
			Assert.True(result.Headers.GetValues("Pragma").Contains("no-cache"),
				"expected no-cache Pragma was not found");
	    }

	    [Fact]
        public void maxage_mustrevalidate_true_headers_correct()
        {
            var client = new HttpClient(_server);
            var result = client.GetAsync(_url + "Get_c50_mustR").Result;

            Assert.Equal(TimeSpan.FromSeconds(50), result.Headers.CacheControl.MaxAge);
            Assert.True(result.Headers.CacheControl.MustRevalidate);
        }

        [Fact]
        public void maxage_private_true_headers_correct()
        {
            var client = new HttpClient(_server);
            var result = client.GetAsync(_url + "Get_c50_private").Result;

            Assert.Equal(TimeSpan.FromSeconds(50), result.Headers.CacheControl.MaxAge);
            Assert.True(result.Headers.CacheControl.Private);
        }

        [Fact]
        public void maxage_mustrevalidate_headers_correct_with_cacheuntil()
        {
            var client = new HttpClient(_server);
            var result = client.GetAsync(_url + "Get_until25012015_1700").Result;
            var clientTimeSpanSeconds = new SpecificTime(2017, 01, 25, 17, 0, 0).Execute(DateTime.Now).ClientTimeSpan.TotalSeconds;
            var resultCacheControlSeconds = ((TimeSpan) result.Headers.CacheControl.MaxAge).TotalSeconds;
            Assert.True(Math.Round(clientTimeSpanSeconds - resultCacheControlSeconds) == 0);
            Assert.False(result.Headers.CacheControl.MustRevalidate);
        }

        [Fact]
        public void maxage_mustrevalidate_headers_correct_with_cacheuntil_today()
        {
            var client = new HttpClient(_server);
            var result = client.GetAsync(_url + "Get_until2355_today").Result;

            Assert.True(Math.Round(new ThisDay(23,55,59).Execute(DateTime.Now).ClientTimeSpan.TotalSeconds - ((TimeSpan)result.Headers.CacheControl.MaxAge).TotalSeconds) == 0);
            Assert.False(result.Headers.CacheControl.MustRevalidate);
        }

        [Fact]
        public void maxage_mustrevalidate_headers_correct_with_cacheuntil_this_month()
        {
            var client = new HttpClient(_server);
            var result = client.GetAsync(_url + "Get_until27_thismonth").Result;

            Assert.True(Math.Round(new ThisMonth(27,0,0,0).Execute(DateTime.Now).ClientTimeSpan.TotalSeconds - ((TimeSpan)result.Headers.CacheControl.MaxAge).TotalSeconds) == 0);
            Assert.False(result.Headers.CacheControl.MustRevalidate);
        }

        [Fact]
        public void maxage_mustrevalidate_headers_correct_with_cacheuntil_this_year()
        {
            var client = new HttpClient(_server);
            var result = client.GetAsync(_url + "Get_until731_thisyear").Result;

            Assert.True(Math.Round(new ThisYear(7, 31, 0, 0, 0).Execute(DateTime.Now).ClientTimeSpan.TotalSeconds - ((TimeSpan)result.Headers.CacheControl.MaxAge).TotalSeconds) == 0);
            Assert.False(result.Headers.CacheControl.MustRevalidate);
        }

        [Fact]
        public void maxage_mustrevalidate_headers_correct_with_cacheuntil_this_year_with_revalidate()
        {
            var client = new HttpClient(_server);
            var result = client.GetAsync(_url + "Get_until731_thisyear_mustrevalidate").Result;

            Assert.True(Math.Round(new ThisYear(7, 31, 0, 0, 0).Execute(DateTime.Now).ClientTimeSpan.TotalSeconds - ((TimeSpan)result.Headers.CacheControl.MaxAge).TotalSeconds) == 0);
            Assert.True(result.Headers.CacheControl.MustRevalidate);
        }

        [Fact]
        public void private_true_headers_correct()
        {
            var client = new HttpClient(_server);
            var result = client.GetAsync(_url + "Get_private").Result;

            Assert.True(result.Headers.CacheControl.Private);
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
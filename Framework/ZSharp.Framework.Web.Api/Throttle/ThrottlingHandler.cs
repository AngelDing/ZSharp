using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ZSharp.Framework.Web.Throttle;

namespace ZSharp.Framework.Web.Api.Throttle
{
    /// <summary>
    /// Throttle message handler
    /// </summary>
    public class ThrottlingHandler : DelegatingHandler
    {
        private readonly IThrottleProcesser processer;

        public ThrottlingHandler(
            ThrottlePolicy policy,
            IPolicyRepository policyRepo = null,
            IThrottleRepository throttleRepo = null)
        {
            var ipAddressParser = new ApiIpAddressParser();
            processer = new ThrottleProcesser(policy, ipAddressParser, policyRepo, throttleRepo);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var identity = SetIndentity(request);
            var result = processer.Process(identity);
            if (result.IsPass == false)
            {
                var code = (HttpStatusCode)result.StatusCode;
                return QuotaExceededResponse(request, result.Content, code, result.RetryAfter);
            }
            // no throttling required
            return base.SendAsync(request, cancellationToken);
        }

        protected virtual RequestIdentity SetIndentity(HttpRequestMessage request)
        {
            var entry = new RequestIdentity();
            entry.ClientIp = processer.GetClientIp(request).ToString();
            entry.Endpoint = request.RequestUri.AbsolutePath.ToLowerInvariant();
            entry.ClientKey = request.Headers.Contains("Authorization-Token") 
                ? request.Headers.GetValues("Authorization-Token").First() 
                : "anon";

            return entry;
        }

        private Task<HttpResponseMessage> QuotaExceededResponse(
            HttpRequestMessage request, object content, HttpStatusCode responseCode, string retryAfter)
        {
            var response = request.CreateResponse(responseCode, content);
            response.Headers.Add("Retry-After", new string[] { retryAfter });
            return Task.FromResult(response);
        }
    }
}

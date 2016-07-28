using Microsoft.Owin;
using System.Linq;
using System.Threading.Tasks;
using ZSharp.Framework.Web.Throttle;

namespace ZSharp.Framework.Web.Api.Throttle
{
    public class ThrottlingMiddleware : OwinMiddleware
    {
        private readonly IThrottleProcesser processer;

        public ThrottlingMiddleware(
            OwinMiddleware next,
            ThrottlePolicy policy,
            IPolicyRepository policyRepo = null,
            IThrottleRepository throttleRepo = null)
            : base(next)
        {
            var ipAddressParser = new ApiIpAddressParser();
            processer = new ThrottleProcesser(policy, ipAddressParser, policyRepo, throttleRepo);
        }     

        public override async Task Invoke(IOwinContext context)
        {
            var response = context.Response;
            var request = context.Request;
            var identity = SetIndentity(request);
            var result = processer.Process(identity);

            if (result.IsPass == false)
            {
                // break execution
                response.OnSendingHeaders(state =>
                {
                    var resp = (OwinResponse)state;
                    resp.Headers.Add("Retry-After", new string[] { result.RetryAfter});
                    resp.StatusCode = result.StatusCode;
                    resp.ReasonPhrase = result.Message;
                }, response);

                return;
            }
            // no throttling required
            await Next.Invoke(context);
        }

        protected virtual RequestIdentity SetIndentity(IOwinRequest request)
        {
            var entry = new RequestIdentity();
            entry.ClientIp = request.RemoteIpAddress;
            entry.Endpoint = request.Uri.AbsolutePath.ToLowerInvariant();
            entry.ClientKey = request.Headers.Keys.Contains("Authorization-Token")
                ? request.Headers.GetValues("Authorization-Token").First()
                : "anon";

            return entry;
        }
    }
}

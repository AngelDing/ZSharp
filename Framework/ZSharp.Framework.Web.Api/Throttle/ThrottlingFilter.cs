using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using ZSharp.Framework.Web.Throttle;

namespace ZSharp.Framework.Web.Api.Throttle
{
    /// <summary>
    /// Throttle action filter
    /// </summary>
    public class ThrottlingFilter : ActionFilterAttribute, IActionFilter
    {
        private readonly IThrottleProcesser processer;
        public ThrottlingFilter()
        {
        }
 
        public ThrottlingFilter(ThrottlePolicy policy, 
            IPolicyRepository policyRepository, 
            IThrottleRepository repository, 
            IThrottleLogger logger)
        {
        }


        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //// add status code and retry after x seconds to response
            //actionContext.Response = QuotaExceededResponse(
            //    actionContext.Request,
            //    string.Format(message, rateLimit, rateLimitPeriod),
            //    QuotaExceededResponseCode,
            //    core.RetryAfterFrom(throttleCounter.Timestamp, rateLimitPeriod));

            base.OnActionExecuting(actionContext);
        } 

        protected virtual HttpResponseMessage QuotaExceededResponse(HttpRequestMessage request, object content, HttpStatusCode responseCode, string retryAfter)
        {
            var response = request.CreateResponse(responseCode, content);
            response.Headers.Add("Retry-After", new string[] { retryAfter });
            return response;
        }        
    }
}

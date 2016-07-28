using System.Linq;
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
 
        public ThrottlingFilter(
            ThrottlePolicy policy,
            IPolicyRepository policyRepo = null,
            IThrottleRepository throttleRepo = null)
        {
            var ipAddressParser = new ApiIpAddressParser();
            processer = new ThrottleProcesser(policy, ipAddressParser, policyRepo, throttleRepo);
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var request = actionContext.Request;
            IEnableThrottlingAttribute attrPolicy = null;
            var applyThrottling = ApplyThrottling(actionContext, out attrPolicy);
            if (applyThrottling)
            {
                var identity = GetIndentity(actionContext);
                var result = processer.Process(identity, attrPolicy);
                if (result.IsPass == false)
                {
                    // add status code and retry after x seconds to response
                    var responseCode = (HttpStatusCode)result.StatusCode;
                    var response = request.CreateResponse(responseCode, result.Content);
                    response.Headers.Add("Retry-After", new string[] { result.RetryAfter });
                    actionContext.Response = response;
                }
            }
            base.OnActionExecuting(actionContext);
        }

        private bool ApplyThrottling(HttpActionContext actionContext, out IEnableThrottlingAttribute attr)
        {
            var applyThrottling = false;
            attr = null;

            var actionDescriptor = actionContext.ActionDescriptor;
            var controllerDescriptor = actionDescriptor.ControllerDescriptor;
            if (controllerDescriptor.GetCustomAttributes<EnableThrottlingAttribute>(true).Any())
            {
                attr = controllerDescriptor.GetCustomAttributes<EnableThrottlingAttribute>(true).First();
                applyThrottling = true;
            }

            if (actionDescriptor.GetCustomAttributes<EnableThrottlingAttribute>(true).Any())
            {
                attr = actionDescriptor.GetCustomAttributes<EnableThrottlingAttribute>(true).First();
                applyThrottling = true;
            }

            // explicit disabled
            if (actionDescriptor.GetCustomAttributes<DisableThrottingAttribute>(true).Any())
            {
                applyThrottling = false;
            }

            return applyThrottling;
        }

        private RequestIdentity GetIndentity(HttpActionContext actionContext)
        {
            var tempRequest = actionContext.Request;
            var entry = new RequestIdentity();
            entry.ClientIp = processer.GetClientIp(tempRequest).ToString();
            entry.Endpoint = tempRequest.RequestUri.AbsolutePath.ToLowerInvariant();
            entry.ClientKey = tempRequest.Headers.Contains("Authorization-Token")
                ? tempRequest.Headers.GetValues("Authorization-Token").First()
                : "anon";

            return entry;
        }
    }
}

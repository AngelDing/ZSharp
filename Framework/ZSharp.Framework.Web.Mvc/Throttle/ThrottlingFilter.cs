using System.Linq;
using System.Web.Mvc;
using ZSharp.Framework.Web.Throttle;

namespace ZSharp.Framework.Web.Mvc.Throttle
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
            var ipAddressParser = new MvcIpAddressParser();
            processer = new ThrottleProcesser(policy, ipAddressParser, policyRepo, throttleRepo);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.IsChildAction == false)
            {
                IEnableThrottlingAttribute attrPolicy = null;
                var applyThrottling = ApplyThrottling(filterContext, out attrPolicy);
                if (applyThrottling)
                {
                    var identity = GetIndentity(filterContext);
                    var result = processer.Process(identity, attrPolicy);
                    if (result.IsPass == false)
                    {
                        if (filterContext.HttpContext.Response.IsRequestBeingRedirected == false)
                        {
                            filterContext.HttpContext.Response.Clear();
                            filterContext.HttpContext.Response.StatusCode = result.StatusCode;
                            filterContext.HttpContext.Response.Headers.Add("Retry-After", result.RetryAfter);
                            filterContext.Result = new ContentResult { Content = result.Message };
                        }
                        else
                        {
                            filterContext.HttpContext.Response.Write(result.Message);
                        }
                        return;
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }

        private RequestIdentity GetIndentity(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var entry = new RequestIdentity();
            entry.ClientIp = processer.GetClientIp(request).ToString();
            entry.ClientKey = request.IsAuthenticated ? "auth" : "anon";

            var rd = request.RequestContext.RouteData;
            string currentAction = rd.GetRequiredString("action");
            string currentController = rd.GetRequiredString("controller");

            switch (processer.Policy.EndpointType)
            {
                case EndpointThrottlingType.AbsolutePath:
                    entry.Endpoint = request.Url.AbsolutePath;
                    break;
                case EndpointThrottlingType.PathAndQuery:
                    entry.Endpoint = request.Url.PathAndQuery;
                    break;
                case EndpointThrottlingType.ControllerAndAction:
                    entry.Endpoint = currentController + "/" + currentAction;
                    break;
                case EndpointThrottlingType.Controller:
                    entry.Endpoint = currentController;
                    break;
                default:
                    break;
            }

            //case insensitive routes
            entry.Endpoint = entry.Endpoint.ToLowerInvariant();

            return entry;
        }

        private bool ApplyThrottling(ActionExecutingContext filterContext, out IEnableThrottlingAttribute attr)
        {
            var applyThrottling = false;
            attr = null;

            if (filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(EnableThrottlingAttribute), true))
            {
                attr = (EnableThrottlingAttribute)filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(EnableThrottlingAttribute), true).First();
                applyThrottling = true;
            }

            if (filterContext.ActionDescriptor.IsDefined(typeof(EnableThrottlingAttribute), true))
            {
                attr = (EnableThrottlingAttribute)filterContext.ActionDescriptor.GetCustomAttributes(typeof(EnableThrottlingAttribute), true).First();
                applyThrottling = true;
            }

            //explicit disabled
            if (filterContext.ActionDescriptor.IsDefined(typeof(DisableThrottingAttribute), true))
            {
                applyThrottling = false;
            }

            return applyThrottling;
        }
    }
}
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using ZSharp.WebApi.Demo.Common.Attributes;

namespace ZSharp.WebApi.Demo.Common.Filters
{
    public class GlobalActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //If you only want to validate the post request.
            if (actionContext.Request.Method != HttpMethod.Post)
            {
                return;
            }

            var passby = actionContext.ActionDescriptor.GetCustomAttributes<BypassModelStateValidationAttribute>().Any() ||
                         actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<BypassModelStateValidationAttribute>().Any();

            if (passby)
            {
                return;
            }

            if (actionContext.ModelState.IsValid == false)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
            }
        }
    }
}
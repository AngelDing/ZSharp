using System.Web.Http.Filters;

namespace ZSharp.Framework.Web.Api.Throttle
{
    public class DisableThrottingAttribute : ActionFilterAttribute, IActionFilter
    {
    }
}

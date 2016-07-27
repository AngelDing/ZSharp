using System.Web.Mvc;

namespace ZSharp.Framework.Web.Mvc.Throttle
{
    public interface IThrottleProcesser
    {
        ThrottleCheckResult ThrottleChecking(ActionExecutingContext context);
    }
}
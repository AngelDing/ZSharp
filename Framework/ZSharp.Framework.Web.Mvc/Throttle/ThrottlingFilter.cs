using GroupTour.Framework.Throttle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Text;
using System.Web.Routing;
namespace ZSharp.Framework.Web.Mvc.Throttle
{
    /// <summary>
    /// Throttle action filter
    /// </summary>
    public class ThrottlingFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.IsChildAction == false)
            {
                var processer = DependencyResolver.Current.GetService<IThrottleProcesser>();
                
                var checkingResult = processer.ThrottleChecking(filterContext);
                 
                if (checkingResult.IsPass == false)
                {

                    if (filterContext.HttpContext.Response.IsRequestBeingRedirected == false)
                    {
                        filterContext.HttpContext.Response.Clear();
                        filterContext.HttpContext.Response.StatusCode = checkingResult.StatusCode;
                        filterContext.HttpContext.Response.Headers.Add(checkingResult.HeadEntry.Key, checkingResult.HeadEntry.Value);
                        filterContext.Result = new ContentResult { Content = checkingResult.Message };
                        
                    }
                    else
                    {
                        filterContext.HttpContext.Response.Write(checkingResult.Message);
                    }
                    return;
                }

            }
            base.OnActionExecuting(filterContext);
        }   
    }
}

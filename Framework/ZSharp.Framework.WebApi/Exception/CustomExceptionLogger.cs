using ZSharp.Framework.Logging;
using System.Net.Http;
using System.Web;
using System.Web.Http.ExceptionHandling;
using System;

namespace ZSharp.Framework.WebApi
{
    public class CustomExceptionLogger : ExceptionLogger
    {
        private const string HttpContextBaseKey = "MS_HttpContext";

        public override void Log(ExceptionLoggerContext context)
        {
            // Retrieve the current HttpContext instance for this request.
            HttpContext httpContext = GetHttpContext(context.Request);

            if (httpContext == null)
            {
                return;
            }            
            var exceptionToRaise = new HttpUnhandledException(message: null, innerException: context.Exception);
            var logger = LogManager.GetLogger(this.GetType());
            var msg = CreateErrorMessage(exceptionToRaise);
            logger.Error(msg, exceptionToRaise);
        }

        private string CreateErrorMessage(System.Exception ex)
        {
            var baseException = ex.GetBaseException();
            return baseException.Message;
        }

        private static HttpContext GetHttpContext(HttpRequestMessage request)
        {
            HttpContextBase contextBase = GetHttpContextBase(request);

            if (contextBase == null)
            {
                return null;
            }

            return ToHttpContext(contextBase);
        }

        private static HttpContextBase GetHttpContextBase(HttpRequestMessage request)
        {
            if (request == null)
            {
                return null;
            }

            object value;

            if (!request.Properties.TryGetValue(HttpContextBaseKey, out value))
            {
                return null;
            }

            return value as HttpContextBase;
        }

        private static HttpContext ToHttpContext(HttpContextBase contextBase)
        {
            return contextBase.ApplicationInstance.Context;
        }
    }
}
using System.Web;

namespace ZSharp.Framework.Utils
{
    public static class SessionHelper
    {
        public static void Set(string key, object value)
        {
            HttpSessionStateBase session = HttpContextHelper.Current.Session;
            session[key] = value;
        }

        public static TValue Get<TValue>(string key)
        {
            HttpSessionStateBase session = HttpContextHelper.Current.Session;
            object value = session[key];
            return (TValue)value;
        }

        public static void Remove(string key)
        {
            HttpSessionStateBase session = HttpContextHelper.Current.Session;
            session.Remove(key);
        }
    }

    public static class HttpContextHelper
    {
        private static object lockObj = new object();
        private static HttpContextBase mockHttpContext;

        /// <summary>
        /// Access the HttpContext using the Abstractions.
        /// </summary>
        public static HttpContextBase Current
        {
            get
            {
                lock (lockObj)
                {
                    if (mockHttpContext == null && HttpContext.Current != null)
                    {
                        return new HttpContextWrapper(HttpContext.Current);
                    }
                }
                return mockHttpContext;
            }
            set
            {
                lock (lockObj)
                {
                    mockHttpContext = value;
                }
            }
        }
    }
}

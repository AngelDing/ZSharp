using System.Collections.Generic;

namespace ZSharp.Framework.Web.Throttle
{
    public class ThrottleProcessResult
    {
        public bool IsPass { get; set; }

        public string Message { get; set; }

        public string RetryAfter { get; set; }

        public int StatusCode { get; set; }

        public object Content { get; set; }
    }
}
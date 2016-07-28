using System;

namespace ZSharp.Framework.Web.Throttle
{
    public class ThrottleLogEntry
    {
        public string RequestId { get; set; }

        public string ClientIp { get; set; }

        public string ClientKey { get; set; }

        public string Endpoint { get; set; }

        public long TotalRequests { get; set; }

        public DateTimeOffset StartPeriod { get; set; }

        public long RateLimit { get; set; }

        public string RateLimitPeriod { get; set; }

        public DateTimeOffset LogDate { get; set; }
    }
}

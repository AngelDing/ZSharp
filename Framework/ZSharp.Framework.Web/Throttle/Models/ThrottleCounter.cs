using System;

namespace ZSharp.Framework.Web.Throttle
{
    /// <summary>
    /// Stores the initial access time and the numbers of calls made from that point
    /// </summary>
    public struct ThrottleCounter
    {
        public DateTimeOffset Timestamp { get; set; }

        public long TotalRequests { get; set; }
    }
}

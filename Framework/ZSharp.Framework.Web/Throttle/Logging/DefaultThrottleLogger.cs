using ZSharp.Framework.Logging;

namespace ZSharp.Framework.Web.Throttle
{
    public class DefaultThrottleLogger : IThrottleLogger
    {
        private readonly ILogger logger;

        public DefaultThrottleLogger()
        {
            logger = LogManager.GetLogger(typeof(DefaultThrottleLogger));
        }

        public void Log(ThrottleLogEntry entry)
        {
            logger.Info("{0} Request {1} from {2} has been throttled (blocked), quota {3}/{4} exceeded by {5}",
                    entry.LogDate,
                    entry.RequestId,
                    entry.ClientIp,
                    entry.RateLimit,
                    entry.RateLimitPeriod,
                    entry.TotalRequests);
        }
    }
}

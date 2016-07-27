
namespace ZSharp.Framework.Web.Throttle
{
    public interface IEnableThrottlingAttribute
    {
        long PerSecond { get; set; }

        long PerMinute { get; set; }

        long PerHour { get; set; }

        long PerDay { get; set; }

        long PerWeek { get; set; }

        long GetLimit(RateLimitPeriod period);
    }
}

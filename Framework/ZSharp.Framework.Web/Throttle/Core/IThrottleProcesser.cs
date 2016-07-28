using System.Net;

namespace ZSharp.Framework.Web.Throttle
{
    public interface IThrottleProcesser
    {
        ThrottlePolicy Policy { get; }

        ThrottleProcessResult Process(RequestIdentity identity, IEnableThrottlingAttribute attrPolicy = null);

        IPAddress GetClientIp(object request);
    }
}

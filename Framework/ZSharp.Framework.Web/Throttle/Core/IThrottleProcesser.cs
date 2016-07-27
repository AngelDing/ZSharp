
namespace ZSharp.Framework.Web.Throttle
{
    public interface IThrottleProcesser
    {
        ThrottleProcessResult Process(object actionContext);
    }
}

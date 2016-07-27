
namespace ZSharp.Framework.Web.Throttle
{
    public enum ThrottlePolicyType : byte
    {
        IpThrottling = 1,
        ClientThrottling,
        EndpointThrottling
    }
}

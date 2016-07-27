using System.Collections.Generic;
using System.Net;

namespace ZSharp.Framework.Web.Throttle
{
    public interface IIpAddressParser
    {
        bool ContainsIp(List<string> ipRules, string clientIp);

        bool ContainsIp(List<string> ipRules, string clientIp, out string rule);

        /// <summary>
        /// 獲取客戶端IP
        /// </summary>
        /// <param name="request">Api：HttpRequestMessage，Mvc：HttpRequestBase</param>
        /// <returns></returns>
        IPAddress GetClientIp(object request);

        IPAddress ParseIp(string ipAddress);
    }
}

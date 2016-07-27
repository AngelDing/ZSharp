using System.Linq;
using System.Net;
using System.Web;
using ZSharp.Framework.Web.Throttle;

namespace ZSharp.Framework.Web.Mvc.Throttle
{
    public class MvcIpAddressParser : IpAddressBaseParser
    {
        public override IPAddress GetClientIp(object request)
        {           
            var requestMsg = request as HttpRequestBase;
            if (requestMsg == null)
            {
                return null;
            }

            string ip = null; 
            try
            {
                if (requestMsg.IsSecureConnection)
                {
                    ip = requestMsg.ServerVariables["REMOTE_ADDR"];
                }

                if (string.IsNullOrEmpty(ip))
                {
                    ip = requestMsg.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (!string.IsNullOrEmpty(ip))
                    {
                        if (ip.IndexOf(",") > 0)
                        {
                            ip = ip.Split(',').Last();
                        }
                    }
                    else
                    {
                        ip = requestMsg.UserHostAddress;
                    }
                }
            }
            catch
            {
                ip = null;
            }

            IPAddress ipAddress;
            var ok = IPAddress.TryParse(ip, out ipAddress);
            if (ok)
            {
                return ipAddress;
            }
            return null;
        }
    }
}

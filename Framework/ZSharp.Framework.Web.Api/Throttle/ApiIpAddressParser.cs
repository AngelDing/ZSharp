using System.Net;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Web;
using ZSharp.Framework.Web.Throttle;

namespace ZSharp.Framework.Web.Api.Throttle
{
    public class ApiIpAddressParser : IpAddressBaseParser
    {
        public override IPAddress GetClientIp(object request)
        {           
            var requestMsg = request as HttpRequestMessage;
            if (requestMsg == null)
            {
                return null;
            }

            IPAddress ipAddress;
            if (requestMsg.Properties.ContainsKey("MS_HttpContext"))
            {
                var ok = IPAddress.TryParse(((HttpContextBase)requestMsg.Properties["MS_HttpContext"]).Request.UserHostAddress, out ipAddress);

                if (ok)
                {
                    return ipAddress;
                }
            }

            if (requestMsg.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                var ok = IPAddress.TryParse(((RemoteEndpointMessageProperty)requestMsg.Properties[RemoteEndpointMessageProperty.Name]).Address, out ipAddress);

                if (ok)
                {
                    return ipAddress;
                }
            }

            if (requestMsg.Properties.ContainsKey("MS_OwinContext"))
            {
                var ok = IPAddress.TryParse(((Microsoft.Owin.OwinContext)requestMsg.Properties["MS_OwinContext"]).Request.RemoteIpAddress, out ipAddress);

                if (ok)
                {
                    return ipAddress;
                }
            }

            return null;
        }
    }
}

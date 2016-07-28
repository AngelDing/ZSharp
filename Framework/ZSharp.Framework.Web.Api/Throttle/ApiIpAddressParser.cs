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

            var ip = string.Empty;
            if (requestMsg.Properties.ContainsKey("MS_HttpContext"))
            {
                ip = ((HttpContextBase)requestMsg.Properties["MS_HttpContext"]).Request.UserHostAddress;
                return ParseIp(ip);              
            }

            if (requestMsg.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                ip = ((RemoteEndpointMessageProperty)requestMsg.Properties[RemoteEndpointMessageProperty.Name]).Address;
                return ParseIp(ip);
            }

            return null;
        }
    }
}

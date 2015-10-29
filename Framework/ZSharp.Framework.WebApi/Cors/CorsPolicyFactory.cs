using System;
using System.Net.Http;
using System.Web.Http.Cors;

namespace ZSharp.Framework.WebApi
{
    /// <summary>
    /// http://www.asp.net/web-api/overview/security/enabling-cross-origin-requests-in-web-api
    /// </summary>
    public class CorsPolicyFactory : ICorsPolicyProviderFactory
    {
        ICorsPolicyProvider _provider = new CorsPolicyAttribute();

        public ICorsPolicyProvider GetCorsPolicyProvider(HttpRequestMessage request)
        {
            return _provider;
        }
    }
}
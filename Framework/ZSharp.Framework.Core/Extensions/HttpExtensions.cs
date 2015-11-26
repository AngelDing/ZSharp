﻿using System.Web;

namespace ZSharp.Framework.Extensions
{    
    public static class HttpExtensions
    {
        private const string HTTP_CLUSTER_VAR = "HTTP_CLUSTER_HTTPS";
        
        /// <summary>
        /// Gets a value which indicates whether the HTTP connection uses secure sockets (HTTPS protocol). 
        /// Works with Cloud's load balancers.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool IsSecureConnection(this HttpRequestBase request)
        {
            return (request.IsSecureConnection || (request.ServerVariables[HTTP_CLUSTER_VAR] != null || request.ServerVariables[HTTP_CLUSTER_VAR] == "on"));
        }
    }
}

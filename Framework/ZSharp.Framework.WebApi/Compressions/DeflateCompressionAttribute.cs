﻿using System.Net.Http;
using System.Web.Http.Filters;

namespace ZSharp.Framework.WebApi
{
    /// <summary>
    /// 指定具體的action，採用壓縮方法，減少帶寬流量
    /// </summary>
    public class DeflateCompressionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actContext)
        {
            var content = actContext.Response.Content;
            var bytes = content == null ? null : content.ReadAsByteArrayAsync().Result;
            var zlibbedContent = bytes == null ? new byte[0] : CompressionHelper.DeflateByte(bytes);

            actContext.Response.Content = new ByteArrayContent(zlibbedContent);
            actContext.Response.Content.Headers.Remove("Content-Type");
            actContext.Response.Content.Headers.Add("Content-encoding", "deflate");
            actContext.Response.Content.Headers.Add("Content-Type", "application/json;charset=utf-8");
            base.OnActionExecuted(actContext);
        }
    }    
}
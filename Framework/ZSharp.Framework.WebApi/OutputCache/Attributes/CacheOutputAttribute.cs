﻿using ZSharp.Framework.WebApi.OutputCache;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ZSharp.Framework.WebApi.OutputCache
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class CacheOutputAttribute : ActionFilterAttribute
    {
        private const string CurrentRequestMediaType = "CacheOutput:CurrentRequestMediaType";
        protected static MediaTypeHeaderValue DefaultMediaType = new MediaTypeHeaderValue("application/json") { CharSet = Encoding.UTF8.HeaderName };

        /// <summary>
        /// Cache enabled only for requests when Thread.CurrentPrincipal is not set
        /// </summary>
        public bool AnonymousOnly { get; set; }

        /// <summary>
        /// Corresponds to MustRevalidate HTTP header - indicates whether the origin server requires revalidation of a cache entry on any subsequent use when the cache entry becomes stale
        /// </summary>
        public bool MustRevalidate { get; set; }

        /// <summary>
        /// Do not vary cache by querystring values
        /// </summary>
        public bool ExcludeQueryStringFromCacheKey { get; set; }

        /// <summary>
        /// How long response should be cached on the server side (in seconds)
        /// </summary>
        public int ServerTimeSpan { get; set; }

        /// <summary>
        /// Corresponds to CacheControl MaxAge HTTP header (in seconds)
        /// </summary>
        public int ClientTimeSpan { get; set; }

        /// <summary>
        /// Corresponds to CacheControl NoCache HTTP header
        /// </summary>
        public bool NoCache { get; set; }

        /// <summary>
        /// Corresponds to CacheControl Private HTTP header. Response can be cached by browser but not by intermediary cache
        /// </summary>
        public bool Private { get; set; }

        /// <summary>
        /// Class used to generate caching keys
        /// </summary>
        public Type CacheKeyGenerator { get; set; }

        // cache repository
        private IApiOutputCache _webApiCache;

        protected virtual void EnsureCache(HttpConfiguration config, HttpRequestMessage req)
        {
            _webApiCache = config.CacheOutputConfiguration().GetCacheOutputProvider(req);
        }

        internal IModelQuery<DateTime, CacheTime> CacheTimeQuery;

        readonly Func<HttpActionContext, bool, bool> _isCachingAllowed = (ac, anonymous) =>
        {
            if (anonymous)
                if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
                    return false;

            return ac.Request.Method == HttpMethod.Get || ac.Request.Method == HttpMethod.Post;
        };

        protected virtual void EnsureCacheTimeQuery()
        {
            if (CacheTimeQuery == null) ResetCacheTimeQuery();
        }

        protected void ResetCacheTimeQuery()
        {
            CacheTimeQuery = new ShortTime(ServerTimeSpan, ClientTimeSpan);
        }

        protected virtual MediaTypeHeaderValue GetExpectedMediaType(HttpConfiguration config, HttpActionContext actionContext)
        {
            MediaTypeHeaderValue responseMediaType = null;

            var negotiator = config.Services.GetService(typeof(IContentNegotiator)) as IContentNegotiator;
            var returnType = actionContext.ActionDescriptor.ReturnType;

            if (negotiator != null && returnType != typeof(HttpResponseMessage) && (returnType != typeof(IHttpActionResult) || typeof(IHttpActionResult).IsAssignableFrom(returnType)))
            {
                var negotiatedResult = negotiator.Negotiate(returnType, actionContext.Request, config.Formatters);

                if (negotiatedResult == null)
                {
                    return DefaultMediaType;
                }

                responseMediaType = negotiatedResult.MediaType;
                if (string.IsNullOrWhiteSpace(responseMediaType.CharSet))
                {
                    responseMediaType.CharSet = Encoding.UTF8.HeaderName;
                }
            }
            else
            {
                if (actionContext.Request.Headers.Accept != null)
                {
                    responseMediaType = actionContext.Request.Headers.Accept.FirstOrDefault();
                    if (responseMediaType == null || !config.Formatters.Any(x => x.SupportedMediaTypes.Contains(responseMediaType)))
                    {
                        return DefaultMediaType;
                    }
                }
            }

            return responseMediaType;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext == null) throw new ArgumentNullException("actionContext");

            if (!_isCachingAllowed(actionContext, AnonymousOnly)) return;

            var config = actionContext.Request.GetConfiguration();

            EnsureCacheTimeQuery();
            EnsureCache(config, actionContext.Request);

            var cacheKeyGenerator = config.CacheOutputConfiguration().GetCacheKeyGenerator(actionContext.Request, CacheKeyGenerator);

            var responseMediaType = GetExpectedMediaType(config, actionContext);
            actionContext.Request.Properties[CurrentRequestMediaType] = responseMediaType;
            var cachekey = cacheKeyGenerator.MakeCacheKey(actionContext, responseMediaType, ExcludeQueryStringFromCacheKey);

            if (!_webApiCache.Contains(cachekey)) return;

            if (actionContext.Request.Headers.IfNoneMatch != null)
            {
                var etag = _webApiCache.Get<string>(cachekey + Constants.EtagKey);
                if (etag != null)
                {
                    if (actionContext.Request.Headers.IfNoneMatch.Any(x => x.Tag == etag))
                    {
                        var time = CacheTimeQuery.Execute(DateTime.Now);
                        var quickResponse = actionContext.Request.CreateResponse(HttpStatusCode.NotModified);
                        ApplyCacheHeaders(quickResponse, time);
                        actionContext.Response = quickResponse;
                        return;
                    }
                }
            }

            var val = _webApiCache.Get<byte[]>(cachekey);
            if (val == null) return;

            var contenttype = _webApiCache.Get<MediaTypeHeaderValue>(cachekey + Constants.ContentTypeKey) ?? new MediaTypeHeaderValue(cachekey.Split(new[] { ':' }, 2)[1]);

            actionContext.Response = actionContext.Request.CreateResponse();
            actionContext.Response.Content = new ByteArrayContent(val);

            actionContext.Response.Content.Headers.ContentType = contenttype;
            var responseEtag = _webApiCache.Get<string>(cachekey + Constants.EtagKey);
            if (responseEtag != null) SetEtag(actionContext.Response, responseEtag);

            var cacheTime = CacheTimeQuery.Execute(DateTime.Now);
            ApplyCacheHeaders(actionContext.Response, cacheTime);
        }

        public override async Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            if (actionExecutedContext.ActionContext.Response == null || !actionExecutedContext.ActionContext.Response.IsSuccessStatusCode) return;

            if (!_isCachingAllowed(actionExecutedContext.ActionContext, AnonymousOnly)) return;

            var cacheTime = CacheTimeQuery.Execute(DateTime.Now);
            if (cacheTime.AbsoluteExpiration > DateTime.Now)
            {
                var httpConfig = actionExecutedContext.Request.GetConfiguration();
                var config = httpConfig.CacheOutputConfiguration();
                var cacheKeyGenerator = config.GetCacheKeyGenerator(actionExecutedContext.Request, CacheKeyGenerator);

                var responseMediaType = actionExecutedContext.Request.Properties[CurrentRequestMediaType] as MediaTypeHeaderValue ?? GetExpectedMediaType(httpConfig, actionExecutedContext.ActionContext);
                var cachekey = cacheKeyGenerator.MakeCacheKey(actionExecutedContext.ActionContext, responseMediaType, ExcludeQueryStringFromCacheKey);

                if (!string.IsNullOrWhiteSpace(cachekey) && !(_webApiCache.Contains(cachekey)))
                {
                    SetEtag(actionExecutedContext.Response, CreateEtag(actionExecutedContext, cachekey, cacheTime));

                    var responseContent = actionExecutedContext.Response.Content;

                    if (responseContent != null)
                    {
                        var baseKey = config.MakeBaseCachekey(actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerType.FullName, actionExecutedContext.ActionContext.ActionDescriptor.ActionName);
                        var contentType = responseContent.Headers.ContentType;
                        string etag = actionExecutedContext.Response.Headers.ETag.Tag;
                        //ConfigureAwait false to avoid deadlocks
                        var content = await responseContent.ReadAsByteArrayAsync().ConfigureAwait(false);

                        responseContent.Headers.Remove("Content-Length");

                        _webApiCache.Add(baseKey, string.Empty, cacheTime.AbsoluteExpiration);
                        _webApiCache.Add(cachekey, content, cacheTime.AbsoluteExpiration, baseKey);


                        _webApiCache.Add(cachekey + Constants.ContentTypeKey,
                                        contentType,
                                        cacheTime.AbsoluteExpiration, baseKey);


                        _webApiCache.Add(cachekey + Constants.EtagKey,
                                        etag,
                                        cacheTime.AbsoluteExpiration, baseKey);
                    }
                }
            }

            ApplyCacheHeaders(actionExecutedContext.ActionContext.Response, cacheTime);
        }

        protected virtual void ApplyCacheHeaders(HttpResponseMessage response, CacheTime cacheTime)
        {
            if (cacheTime.ClientTimeSpan > TimeSpan.Zero || MustRevalidate || Private)
            {
                var cachecontrol = new CacheControlHeaderValue
                                       {
                                           MaxAge = cacheTime.ClientTimeSpan,
                                           MustRevalidate = MustRevalidate,
                                           Private = Private
                                       };

                response.Headers.CacheControl = cachecontrol;
            }
            else if (NoCache)
            {
                response.Headers.CacheControl = new CacheControlHeaderValue { NoCache = true };
                response.Headers.Add("Pragma", "no-cache");
            }
        }

        protected virtual string CreateEtag(HttpActionExecutedContext actionExecutedContext, string cachekey, CacheTime cacheTime)
        {
            return Guid.NewGuid().ToString();
        }

        private static void SetEtag(HttpResponseMessage message, string etag)
        {
            if (etag != null)
            {
                var eTag = new EntityTagHeaderValue(@"""" + etag.Replace("\"", string.Empty) + @"""");
                message.Headers.ETag = eTag;
            }
        }
    }
}

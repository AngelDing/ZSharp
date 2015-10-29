using System.Net.Http.Headers;
using System.Web.Http.Controllers;

namespace ZSharp.Framework.WebApi.OutputCache
{
    public interface ICacheKeyGenerator
    {
        string MakeCacheKey(HttpActionContext context, MediaTypeHeaderValue mediaType, bool excludeQueryString = false);
    }
}

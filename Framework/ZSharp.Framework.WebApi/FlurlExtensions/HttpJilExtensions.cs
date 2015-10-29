using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Flurl.Http
{
    public static class HttpJilExtensions
    {
        #region FlurlClient GetJilJson using JilJson
        /// <summary>
        /// Sends an asynchronous GET request.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A Task whose result is the JSON response body deserialized to an object of type T.</returns>
        public static Task<T> GetJilJsonAsync<T>(this FlurlClient client, CancellationToken cancellationToken)
        {
            return client.SendAsync(HttpMethod.Get, cancellationToken: cancellationToken).ReceiveJil<T>();
        }

        /// <summary>
        /// Sends an asynchronous GET request.
        /// </summary>
        /// <returns>A Task whose result is the JSON response body deserialized to an object of type T.</returns>
        public static Task<T> GetJilJsonAsync<T>(this FlurlClient client)
        {
            return client.SendAsync(HttpMethod.Get).ReceiveJil<T>();
        }

        /// <summary>
        /// Sends an asynchronous GET request.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A Task whose result is the JSON response body deserialized to a dynamic.</returns>
        public static Task<dynamic> GetJilJsonAsync(this FlurlClient client, CancellationToken cancellationToken)
        {
            return client.SendAsync(HttpMethod.Get, cancellationToken: cancellationToken).ReceiveJil();
        }

        /// <summary>
        /// Sends an asynchronous GET request.
        /// </summary>
        /// <returns>A Task whose result is the JSON response body deserialized to a dynamic.</returns>
        public static Task<dynamic> GetJilJsonAsync(this FlurlClient client)
        {
            return client.SendAsync(HttpMethod.Get).ReceiveJil();
        }

        /// <summary>
        /// Sends an asynchronous GET request.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A Task whose result is the JSON response body deserialized to a list of dynamics.</returns>
        public static Task<IList<dynamic>> GetJilJsonListAsync(this FlurlClient client, CancellationToken cancellationToken)
        {
            return client.SendAsync(HttpMethod.Get, cancellationToken: cancellationToken).ReceiveJilList();
        }

        /// <summary>
        /// Sends an asynchronous GET request.
        /// </summary>
        /// <returns>A Task whose result is the JSON response body deserialized to a list of dynamics.</returns>
        public static Task<IList<dynamic>> GetJsonListAsync(this FlurlClient client)
        {
            return client.SendAsync(HttpMethod.Get).ReceiveJilList();
        }

        #endregion

        #region url GetJilJson using JilJson
        /// <summary>
        /// Creates a FlurlClient from the URL and sends an asynchronous GET request.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A Task whose result is the JSON response body deserialized to an object of type T.</returns>
        public static Task<T> GetJilJsonAsync<T>(this Url url, CancellationToken cancellationToken)
        {
            return new FlurlClient(url, false).SendAsync(HttpMethod.Get, cancellationToken: cancellationToken).ReceiveJil<T>();
        }

        /// <summary>
        /// Creates a FlurlClient from the URL and sends an asynchronous GET request.
        /// </summary>
        /// <returns>A Task whose result is the JSON response body deserialized to an object of type T.</returns>
        public static Task<T> GetJilJsonAsync<T>(this Url url)
        {
            return new FlurlClient(url, false).SendAsync(HttpMethod.Get).ReceiveJil<T>();
        }

        /// <summary>
        /// Creates a FlurlClient from the URL and sends an asynchronous GET request.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A Task whose result is the JSON response body deserialized to a dynamic.</returns>
        public static Task<dynamic> GetJilJsonAsync(this Url url, CancellationToken cancellationToken)
        {
            return new FlurlClient(url, false).SendAsync(HttpMethod.Get, cancellationToken: cancellationToken).ReceiveJil();
        }

        /// <summary>
        /// Creates a FlurlClient from the URL and sends an asynchronous GET request.
        /// </summary>
        /// <returns>A Task whose result is the JSON response body deserialized to a dynamic.</returns>
        public static Task<dynamic> GetJilJsonAsync(this Url url)
        {
            return new FlurlClient(url, false).SendAsync(HttpMethod.Get).ReceiveJil();
        }

        /// <summary>
        /// Creates a FlurlClient from the URL and sends an asynchronous GET request.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A Task whose result is the JSON response body deserialized to a list of dynamics.</returns>
        public static Task<IList<dynamic>> GetJilJsonListAsync(this Url url, CancellationToken cancellationToken)
        {
            return new FlurlClient(url, false).SendAsync(HttpMethod.Get, cancellationToken: cancellationToken).ReceiveJilList();
        }

        /// <summary>
        /// Creates a FlurlClient from the URL and sends an asynchronous GET request.
        /// </summary>
        /// <returns>A Task whose result is the JSON response body deserialized to a list of dynamics.</returns>
        public static Task<IList<dynamic>> GetJilJsonListAsync(this Url url)
        {
            return new FlurlClient(url, false).SendAsync(HttpMethod.Get).ReceiveJilList();
        }

        #endregion

        #region string GetJilJson using JilJson

        /// <summary>
        /// Creates a FlurlClient from the URL and sends an asynchronous GET request.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A Task whose result is the JSON response body deserialized to an object of type T.</returns>
        public static Task<T> GetJilJsonAsync<T>(this string url, CancellationToken cancellationToken)
        {
            return new FlurlClient(url, false).SendAsync(HttpMethod.Get, cancellationToken: cancellationToken).ReceiveJil<T>();
        }

        /// <summary>
        /// Creates a FlurlClient from the URL and sends an asynchronous GET request.
        /// </summary>
        /// <returns>A Task whose result is the JSON response body deserialized to an object of type T.</returns>
        public static Task<T> GetJilJsonAsync<T>(this string url)
        {
            return new FlurlClient(url, false).SendAsync(HttpMethod.Get).ReceiveJil<T>();
        }

        /// <summary>
        /// Creates a FlurlClient from the URL and sends an asynchronous GET request.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A Task whose result is the JSON response body deserialized to a dynamic.</returns>
        public static Task<dynamic> GetJilJsonAsync(this string url, CancellationToken cancellationToken)
        {
            return new FlurlClient(url, false).SendAsync(HttpMethod.Get, cancellationToken: cancellationToken).ReceiveJil();
        }

        /// <summary>
        /// Creates a FlurlClient from the URL and sends an asynchronous GET request.
        /// </summary>
        /// <returns>A Task whose result is the JSON response body deserialized to a dynamic.</returns>
        public static Task<dynamic> GetJilJsonAsync(this string url)
        {
            return new FlurlClient(url, false).SendAsync(HttpMethod.Get).ReceiveJil();
        }

        /// <summary>
        /// Creates a FlurlClient from the URL and sends an asynchronous GET request.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A Task whose result is the JSON response body deserialized to a list of dynamics.</returns>
        public static Task<IList<dynamic>> GetJilJsonListAsync(this string url, CancellationToken cancellationToken)
        {
            return new FlurlClient(url, false).SendAsync(HttpMethod.Get, cancellationToken: cancellationToken).ReceiveJilList();
        }

        /// <summary>
        /// Creates a FlurlClient from the URL and sends an asynchronous GET request.
        /// </summary>
        /// <returns>A Task whose result is the JSON response body deserialized to a list of dynamics.</returns>
        public static Task<IList<dynamic>> GetJilJsonListAsync(this string url)
        {
            return new FlurlClient(url, false).SendAsync(HttpMethod.Get).ReceiveJilList();
        }
        #endregion

        #region PostJilJson

        /// <summary>
        /// Sends an asynchronous POST request.
        /// </summary>
        /// <param name="data">Contents of the request body.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A Task whose result is the received HttpResponseMessage.</returns>
        public static Task<HttpResponseMessage> PostJilJsonAsync(this FlurlClient client, object data, CancellationToken cancellationToken)
        {
            return client.SendAsync(HttpMethod.Post, content: new CapturedJilJsonContent(data), cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Sends an asynchronous POST request.
        /// </summary>
        /// <param name="data">Contents of the request body.</param>
        /// <returns>A Task whose result is the received HttpResponseMessage.</returns>
        public static Task<HttpResponseMessage> PostJilJsonAsync(this FlurlClient client, object data)
        {
            return client.SendAsync(HttpMethod.Post, content: new CapturedJilJsonContent(data));
        }

        /// <summary>
        /// Creates a FlurlClient from the URL and sends an asynchronous POST request.
        /// </summary>
        /// <param name="data">Contents of the request body.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A Task whose result is the received HttpResponseMessage.</returns>
        public static Task<HttpResponseMessage> PostJilJsonAsync(this Url url, object data, CancellationToken cancellationToken)
        {
            return new FlurlClient(url, false).SendAsync(HttpMethod.Post, content: new CapturedJilJsonContent(data), cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Creates a FlurlClient from the URL and sends an asynchronous POST request.
        /// </summary>
        /// <param name="data">Contents of the request body.</param>
        /// <returns>A Task whose result is the received HttpResponseMessage.</returns>
        public static Task<HttpResponseMessage> PostJilJsonAsync(this Url url, object data)
        {
            return new FlurlClient(url, false).SendAsync(HttpMethod.Post, content: new CapturedJilJsonContent(data));
        }

        /// <summary>
        /// Creates a FlurlClient from the URL and sends an asynchronous POST request.
        /// </summary>
        /// <param name="data">Contents of the request body.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A Task whose result is the received HttpResponseMessage.</returns>
        public static Task<HttpResponseMessage> PostJilJsonAsync(this string url, object data, CancellationToken cancellationToken)
        {
            return new FlurlClient(url, false).SendAsync(HttpMethod.Post, content: new CapturedJilJsonContent(data), cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Creates a FlurlClient from the URL and sends an asynchronous POST request.
        /// </summary>
        /// <param name="data">Contents of the request body.</param>
        /// <returns>A Task whose result is the received HttpResponseMessage.</returns>
        public static Task<HttpResponseMessage> PostJilJsonAsync(this string url, object data)
        {
            return new FlurlClient(url, false).SendAsync(HttpMethod.Post, content: new CapturedJilJsonContent(data));
        }
        #endregion

        #region PutJilJson

        /// <summary>
        /// Sends an asynchronous PUT request.
        /// </summary>
        /// <param name="data">Contents of the request body.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A Task whose result is the received HttpResponseMessage.</returns>
        public static Task<HttpResponseMessage> PutJilJsonAsync(this FlurlClient client, object data, CancellationToken cancellationToken)
        {
            return client.SendAsync(HttpMethod.Put, content: new CapturedJilJsonContent(data), cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Sends an asynchronous PUT request.
        /// </summary>
        /// <param name="data">Contents of the request body.</param>
        /// <returns>A Task whose result is the received HttpResponseMessage.</returns>
        public static Task<HttpResponseMessage> PutJilJsonAsync(this FlurlClient client, object data)
        {
            return client.SendAsync(HttpMethod.Put, content: new CapturedJilJsonContent(data));
        }

        /// <summary>
        /// Creates a FlurlClient from the URL and sends an asynchronous PUT request.
        /// </summary>
        /// <param name="data">Contents of the request body.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A Task whose result is the received HttpResponseMessage.</returns>
        public static Task<HttpResponseMessage> PutJilJsonAsync(this Url url, object data, CancellationToken cancellationToken)
        {
            return new FlurlClient(url, false).SendAsync(HttpMethod.Put, content: new CapturedJilJsonContent(data), cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Creates a FlurlClient from the URL and sends an asynchronous PUT request.
        /// </summary>
        /// <param name="data">Contents of the request body.</param>
        /// <returns>A Task whose result is the received HttpResponseMessage.</returns>
        public static Task<HttpResponseMessage> PutJilJsonAsync(this Url url, object data)
        {
            return new FlurlClient(url, false).SendAsync(HttpMethod.Put, content: new CapturedJilJsonContent(data));
        }

        /// <summary>
        /// Creates a FlurlClient from the URL and sends an asynchronous PUT request.
        /// </summary>
        /// <param name="data">Contents of the request body.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A Task whose result is the received HttpResponseMessage.</returns>
        public static Task<HttpResponseMessage> PutJilJsonAsync(this string url, object data, CancellationToken cancellationToken)
        {
            return new FlurlClient(url, false).SendAsync(HttpMethod.Put, content: new CapturedJilJsonContent(data), cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Creates a FlurlClient from the URL and sends an asynchronous PUT request.
        /// </summary>
        /// <param name="data">Contents of the request body.</param>
        /// <returns>A Task whose result is the received HttpResponseMessage.</returns>
        public static Task<HttpResponseMessage> PutJilJsonAsync(this string url, object data)
        {
            return new FlurlClient(url, false).SendAsync(HttpMethod.Put, content: new CapturedJilJsonContent(data));
        }
        #endregion

        #region PatchJilJson
        /// <summary>
        /// Sends an asynchronous PATCH request.
        /// </summary>
        /// <param name="data">Contents of the request body.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A Task whose result is the received HttpResponseMessage.</returns>
        public static Task<HttpResponseMessage> PatchJilJsonAsync(this FlurlClient client, object data, CancellationToken cancellationToken)
        {
            return client.SendAsync(new HttpMethod("PATCH"), content: new CapturedJilJsonContent(data), cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Sends an asynchronous PATCH request.
        /// </summary>
        /// <param name="data">Contents of the request body.</param>
        /// <returns>A Task whose result is the received HttpResponseMessage.</returns>
        public static Task<HttpResponseMessage> PatchJilJsonAsync(this FlurlClient client, object data)
        {
            return client.SendAsync(new HttpMethod("PATCH"), content: new CapturedJilJsonContent(data));
        }

        /// <summary>
        /// Creates a FlurlClient from the URL and sends an asynchronous PATCH request.
        /// </summary>
        /// <param name="data">Contents of the request body.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A Task whose result is the received HttpResponseMessage.</returns>
        public static Task<HttpResponseMessage> PatchJilJsonAsync(this Url url, object data, CancellationToken cancellationToken)
        {
            return new FlurlClient(url, false).SendAsync(new HttpMethod("PATCH"), content: new CapturedJilJsonContent(data), cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Creates a FlurlClient from the URL and sends an asynchronous PATCH request.
        /// </summary>
        /// <param name="data">Contents of the request body.</param>
        /// <returns>A Task whose result is the received HttpResponseMessage.</returns>
        public static Task<HttpResponseMessage> PatchJilJsonAsync(this Url url, object data)
        {
            return new FlurlClient(url, false).SendAsync(new HttpMethod("PATCH"), content: new CapturedJilJsonContent(data));
        }

        /// <summary>
        /// Creates a FlurlClient from the URL and sends an asynchronous PATCH request.
        /// </summary>
        /// <param name="data">Contents of the request body.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A Task whose result is the received HttpResponseMessage.</returns>
        public static Task<HttpResponseMessage> PatchJilJsonAsync(this string url, object data, CancellationToken cancellationToken)
        {
            return new FlurlClient(url, false).SendAsync(new HttpMethod("PATCH"), content: new CapturedJilJsonContent(data), cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Creates a FlurlClient from the URL and sends an asynchronous PATCH request.
        /// </summary>
        /// <param name="data">Contents of the request body.</param>
        /// <returns>A Task whose result is the received HttpResponseMessage.</returns>
        public static Task<HttpResponseMessage> PatchJilJsonAsync(this string url, object data)
        {
            return new FlurlClient(url, false).SendAsync(new HttpMethod("PATCH"), content: new CapturedJilJsonContent(data));
        }
        #endregion
    }
}

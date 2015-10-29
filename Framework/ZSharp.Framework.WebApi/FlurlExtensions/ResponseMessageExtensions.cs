using Jil;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Flurl.Http
{
    public static class ResponseMessageExtensions
    {
        public static async Task<T> ReceiveJil<T>(this Task<HttpResponseMessage> response)
        {

            using (var stream = await response.ReceiveStream().ConfigureAwait(false))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return JSON.Deserialize<T>(reader.ReadToEnd(), new Options(dateFormat: DateTimeFormat.ISO8601, includeInherited: true));
                }
            }
        }
        public static async Task<dynamic> ReceiveJil(this Task<HttpResponseMessage> response)
        {
            using (var stream = await response.ReceiveStream().ConfigureAwait(false))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return JSON.DeserializeDynamic(reader.ReadToEnd(), new Options(dateFormat: DateTimeFormat.ISO8601, includeInherited: true));
                }
            }
        }

        public static async Task<IList<dynamic>> ReceiveJilList(this Task<HttpResponseMessage> response)
        {
            using (var stream = await response.ReceiveStream().ConfigureAwait(false))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return JSON.Deserialize<IList<dynamic>>(reader.ReadToEnd(), new Options(dateFormat: DateTimeFormat.ISO8601, includeInherited: true));
                }
            }
        }
    }
}

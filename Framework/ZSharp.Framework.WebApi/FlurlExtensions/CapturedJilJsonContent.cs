using Flurl.Http.Content;
using Jil;
using System.Text;

namespace Flurl.Http
{
    public class CapturedJilJsonContent : CapturedStringContent
    {
        public CapturedJilJsonContent(object data)
            : base(JSON.SerializeDynamic(data, new Options(dateFormat: DateTimeFormat.ISO8601, includeInherited: true)), Encoding.UTF8, "application/json")
        {
        }
    }
}
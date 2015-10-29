using Jil;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ZSharp.Framework.WebApi
{
    /// <summary>
    /// 可用於替換默認的Newtonsoft.Json序列化機制，號稱第一快的json序列化器
    /// http://blog.developers.ba/replace-json-net-jil-json-serializer-asp-net-web-api/
    /// </summary>
    public class JilFormatter : MediaTypeFormatter
    {
        private readonly Options _jilOptions;
        public JilFormatter() : this(DateTimeFormat.ISO8601)
        {
        }
        public JilFormatter(DateTimeFormat dateTimeFormat)
        {
            _jilOptions = new Options(dateFormat: dateTimeFormat, includeInherited: true);
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));

            SupportedEncodings.Add(new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true));
            SupportedEncodings.Add(new UnicodeEncoding(bigEndian: false, byteOrderMark: true, throwOnInvalidBytes: true));
        }
        public override bool CanReadType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            return true;
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            return Task.FromResult(this.DeserializeFromStream(type, readStream));
        }

        private object DeserializeFromStream(Type type, Stream readStream)
        {
            try
            {
                using (var reader = new StreamReader(readStream))
                {
                    MethodInfo method = typeof(JSON).GetMethod("Deserialize", new Type[] { typeof(TextReader), typeof(Options) });
                    MethodInfo generic = method.MakeGenericMethod(type);
                    return generic.Invoke(this, new object[] { reader, _jilOptions });
                }
            }
            catch
            {
                return null;
            }
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            var streamWriter = new StreamWriter(writeStream);

            JSON.Serialize(value, streamWriter, _jilOptions);
            //当使用流写文件时，数据流会先进入到缓冲区中，而不会立刻写入文件，当执行Flush方法后，缓冲区的数据流会立即注入基础流，另外，
            //此處不能用using來釋放streamWriter，釋放后writeStream也會清空，後續壓縮DeflateCompressionAttribute會讀不到流數據
            // Do not dispose the StreamWriter as it will also dispose the writeStream which must stay open.
            // StreamWriter itself doesn't have unmanaged resources and the writeStream is closed by the WebAPI Framework.
            streamWriter.Flush();
            return TaskHelpers.Completed();
            //return Task.FromResult(writeStream);
        }
    }
}

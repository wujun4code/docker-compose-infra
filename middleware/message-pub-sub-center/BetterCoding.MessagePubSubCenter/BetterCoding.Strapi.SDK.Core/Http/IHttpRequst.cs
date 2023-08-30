using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterCoding.Strapi.SDK.Core.Http
{
    public interface IHttpRequst
    {
        Uri Target => new Uri(new Uri(Resource), Path);

        string Resource { get; set; }

        string Path { get; set; }

        IList<KeyValuePair<string, string>> Headers { get; set; }

        Stream Data { get; set; }

        string Method { get; set; }
    }

    public class HttpRequst : IHttpRequst 
    {
        public Uri Target => new Uri(new Uri(Resource), Path);

        public string Resource { get; set; }

        public string Path { get; set; }

        public IList<KeyValuePair<string, string>> Headers { get; set; }

        public IDictionary<string, object> DataObject { get; private set; }
        public virtual Stream Data
        {
            get => Data ??= DataObject is { } ? new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(DataObject))) : default;
            set => Data = value;
        }

        public string Method { get; set; }

        public HttpRequst(string relativeUri, 
            string method,
            string sessionToken = null, 
            IList<KeyValuePair<string, string>> headers = null, 
            Stream stream = null, string contentType = null)
        {
            Path = relativeUri;
            Method = method;
            Data = stream;
            Headers = new List<KeyValuePair<string, string>>(headers ?? Enumerable.Empty<KeyValuePair<string, string>>());

            if (!String.IsNullOrEmpty(sessionToken))
            {
                Headers.Add(new KeyValuePair<string, string>("Authorization", sessionToken));
            }

            if (!String.IsNullOrEmpty(contentType))
            {
                Headers.Add(new KeyValuePair<string, string>("Content-Type", contentType));
            }
        }

        public HttpRequst(HttpRequst other)
        {
            Resource = other.Resource;
            Path = other.Path;
            Method = other.Method;
            DataObject = other.DataObject;
            Headers = new List<KeyValuePair<string, string>>(other.Headers);
            Data = other.Data;
        }
    }
}

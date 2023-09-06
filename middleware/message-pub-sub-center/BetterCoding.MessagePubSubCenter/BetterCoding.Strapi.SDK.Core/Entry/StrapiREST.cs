using BetterCoding.Strapi.SDK.Core.Http;
using BetterCoding.Strapi.SDK.Core.Services;

namespace BetterCoding.Strapi.SDK.Core.Entry
{
    public class StrapiREST
    {
        public IServiceHub Services { get; internal set; }

        public StrapiREST(IServiceHub serviceHub = default)
        {
            Services = StrapiClient.Instance.Services;
        }

        public async Task LogInAsync(string username, string password)
        {
            var request = CreateRequest("/api/auth/local", "POST",
                data: new Dictionary<string, object>
                {
                    {"identifier", username},
                    {"password", password}
                });

            var result = await ExecuteAsync(request);
            var user = Services.UserDecoder.Decode(result, Services);
        }

        public HttpRequest CreateRequest(
            string relativeUri,
            string method,
            IDictionary<string, object> data = null)
        {
            var presetHeaders = new Dictionary<string, string>
            {

            };

            var request = new HttpRequest(relativeUri, method,
               data: data,
               headers: presetHeaders.ToList())
            {
                Resource = Services.ServerConfiguration.ServerURI
            };

            return request;
        }

        public async Task<IDictionary<string, object>> ExecuteAsync(HttpRequest request)
        {
            var response = await Services.WebClient.ExecuteAsync(request);
            var content = response.Item2;
            var statusCode = (int)response.Item1;
            if (statusCode < 200 || statusCode > 299)
            {
                throw new HttpRequestException();
            }

            var contentJson = Services.JsonTool.Parse(content, Services);
            return contentJson;
        }



        //public async Task<T> Get<T, TId>(string entryName, TId id) 
        //{
        //    var request = new HttpRequest("");
        //}

    }
}

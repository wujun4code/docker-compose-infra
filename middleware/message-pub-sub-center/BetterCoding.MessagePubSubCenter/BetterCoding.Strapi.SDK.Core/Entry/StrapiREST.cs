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

            var response = await Services.WebClient.ExecuteAsync(request);
            if (response.Item1 == System.Net.HttpStatusCode.OK)
            {
                var user = new { response.Item2 };

            }
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



        //public async Task<T> Get<T, TId>(string entryName, TId id) 
        //{
        //    var request = new HttpRequest("");
        //}

    }
}

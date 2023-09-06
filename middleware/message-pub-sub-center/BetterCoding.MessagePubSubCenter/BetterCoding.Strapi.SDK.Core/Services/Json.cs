using Newtonsoft.Json;

namespace BetterCoding.Strapi.SDK.Core.Services
{
    public interface IJsonTool
    {
        IDictionary<string, object> Parse(string json, IServiceHub serviceHub);
    }

    public class JsonTool : IJsonTool
    {
        public IDictionary<string, object> Parse(string json, IServiceHub serviceHub)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        }
    }
}

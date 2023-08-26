using Microsoft.Extensions.Configuration;

namespace BetterCoding.MessagePubSubCenter.Services
{
    public interface IAPIKeyAuthenticate
    {
        bool IsValidApiKey(string userApiKey);
    }

    public class APIKeyAuthenticate : IAPIKeyAuthenticate
    {
        public readonly IConfiguration _configuration;
        public APIKeyAuthenticate(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool IsValidApiKey(string userApiKey)
        {
            var configed = _configuration.GetValue<string>("APIKey");
            if (string.IsNullOrEmpty(configed) || string.IsNullOrEmpty(userApiKey))
                return false;
            return configed.Equals(userApiKey);
        }
    }
}

using BetterCoding.Strapi.SDK.Core.Services;

namespace BetterCoding.Strapi.SDK.Core
{
    public class StrapiClient
    {
        public class StrapiServerConfiguration
        {
            public string ServerURI { get; set; }
            public string APIToken { get; set; }
        }

        public StrapiServerConfiguration ServerConfiguration { get; set; }
        public IServiceHub Services { get; internal set; }

        public StrapiClient(StrapiServerConfiguration configuration = default, IServiceHub serviceHub = default)
        {
            ServerConfiguration = configuration is null ? new StrapiServerConfiguration() : configuration;
            Services = serviceHub is null ? new ServiceHub { } : serviceHub;
        }
    }
}

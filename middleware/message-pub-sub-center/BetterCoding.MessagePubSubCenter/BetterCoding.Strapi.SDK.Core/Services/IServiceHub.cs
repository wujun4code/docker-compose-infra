// Ignore Spelling: Strapi

using BetterCoding.Strapi.SDK.Core.Http;
using BetterCoding.Strapi.SDK.Core.User;
using BetterCoding.Strapi.SDK.Core.Webhook;

namespace BetterCoding.Strapi.SDK.Core.Services
{
    public interface IServiceHub
    {
        IDataDecoder Decoder { get; }
        IWebhookEventCoder WebhookEventCoder { get; }
        IWebhookEventClassMapping WebhookEventClassMapping { get; }
        IQueryStringEncoder QueryStringEncoder { get; }
        IWebClient WebClient { get; }
        StrapiServerConfiguration ServerConfiguration { get; }
        IJsonTool JsonTool { get; }
        IUserDecoder UserDecoder { get; }
    }

    public class ServiceHub : IServiceHub
    {
        public ServiceHub(StrapiServerConfiguration strapiServerConfiguration)
        {
            ServerConfiguration = strapiServerConfiguration;
        }
        public IDataDecoder Decoder => new DataDecode();
        public IWebhookEventCoder WebhookEventCoder => new WebhookEventCoder();
        public IWebhookEventClassMapping WebhookEventClassMapping => new WebhookEventClassMapping();
        public IQueryStringEncoder QueryStringEncoder => new QueryStringEncoder();
        public IWebClient WebClient => new UniversalWebClient();
        public StrapiServerConfiguration ServerConfiguration { get; internal set; }
        public IJsonTool JsonTool => new JsonTool();
        public IUserDecoder UserDecoder => new UserDecoder();
    }
}

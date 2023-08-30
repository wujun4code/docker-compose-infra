// Ignore Spelling: Strapi

using BetterCoding.Strapi.SDK.Core.Webhook;

namespace BetterCoding.Strapi.SDK.Core.Services
{
    public interface IServiceHub
    {
        IDataDecoder Decoder { get; }
        IWebhookEventCoder WebhookEventCoder { get; }
        IWebhookEventClassMapping WebhookEventClassMapping { get; }
        IQueryStringEncoder QueryStringEncoder { get; }
    }

    public class ServiceHub : IServiceHub
    {
        public IDataDecoder Decoder => new DataDecode();
        public IWebhookEventCoder WebhookEventCoder => new WebhookEventCoder();
        public IWebhookEventClassMapping WebhookEventClassMapping => new WebhookEventClassMapping();
        public IQueryStringEncoder QueryStringEncoder => new QueryStringEncoder();
    }
}

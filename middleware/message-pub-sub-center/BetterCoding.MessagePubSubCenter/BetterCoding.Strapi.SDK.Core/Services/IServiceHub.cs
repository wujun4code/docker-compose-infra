// Ignore Spelling: Strapi

using BetterCoding.Strapi.SDK.Core.Webhook;

namespace BetterCoding.Strapi.SDK.Core.Services
{
    public interface IServiceHub
    {
        IDataDecoder Decoder { get; }
        IWebhookEventCoder WebhookEventCoder { get; }
    }

    public class ServiceHub : IServiceHub
    {
        public IDataDecoder Decoder => new DataDecode();
        public IWebhookEventCoder WebhookEventCoder => new WebhookEventCoder();
    }
}
